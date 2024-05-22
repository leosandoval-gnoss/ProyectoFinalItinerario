using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using System.Xml;
using System.Text;
using Newtonsoft.Json.Linq;
using GnossBase;
using Gnoss.ApiWrapper.Helpers;
using System.Text.Json;
using Newtonsoft.Json.Schema;
using static Gnoss.ApiWrapper.ApiModel.SparqlObject;
using System.Globalization;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using Gnoss.ApiWrapper.Interfaces;
using System;
using VDS.RDF.Query.Algebra;
using Newtonsoft.Json;
using ObraleoOntology;
using EscritorleoOntology;
using GenreleoOntology;

internal class Program
{
    public static int cargaObras = 0;
    public static int errorObras = 0;
    private static void Main(string[] args)
    {
        #region Conexión y datos de la comunidad
        string pathOAuth = @"Config\oAuth.config";
        ResourceApi mResourceApi = new ResourceApi(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, pathOAuth));
        #endregion Conexión con la comunidad


        #region Carga de datos
        cargaMasiva(mResourceApi);
        #endregion
    }


    private static void cargaMasiva(ResourceApi mResourceApi)
    {
        int cargasAutores = 0;
        int errorAutores = 0;
        
        string escritroesPath = "Carga/cargaEscritores.json";
        string obrasPath = "Carga/cargaObras.json";

        JArray jArray = JArray.Parse(File.ReadAllText(escritroesPath));
        JArray obrasArray = JArray.Parse(File.ReadAllText(obrasPath));
        foreach (JObject item in jArray.Children<JObject>())
        {
            string[] obraUris = item.Value<string>("obras").Split(" || ");
            string autor = item.Value<string>("autor");
            string genero = item.Value<string>("genero");
            string fechaValue = item["fechaNacimiento"]?.ToString();
            DateTime fechaNacimiento = DateTime.Now;
            DateTime.TryParse(fechaValue, out fechaNacimiento);
            string[] pais = item.Value<string>("lugarNacimiento").Split(" || ");
            string[] ocupaciones = item.Value<string>("ocupaciones").Split(" || ");
            string[] movimiento = item.Value<string>("movimientos").Split(" || ");
            string[] premio = item.Value<string>("premio").Split(" || ");
            string imagen = item.Value<string>("autorFoto");

            Person escritor = new();

            escritor.Schema_name = autor;
            escritor.Schema_gender = genero;
            
            escritor.Schema_birthDate = fechaNacimiento;
             escritor.Schema_countryOfOrigin = new List<string>(pais);
            if(!string.IsNullOrEmpty(imagen)) escritor.Schema_image = imagen;
            if (!string.IsNullOrEmpty(ocupaciones[0])) escritor.Schema_occupation = new List<string>(ocupaciones);
            if (!string.IsNullOrEmpty(movimiento[0])) escritor.Schema_movement = new List<string>(movimiento);
            if (!string.IsNullOrEmpty(premio[0])) escritor.Schema_awards = new List<string>(premio);
            if (!string.IsNullOrEmpty(obraUris[0]))
            {
                List<string> obras = comprobarObras(obraUris, obrasArray, mResourceApi);
                escritor.IdsTry_authorOf = obras;
            };
            // Cargar el recurso principal
            mResourceApi.ChangeOntology("escritorleo.owl");
            ComplexOntologyResource resorceToLoad = escritor.ToGnossApiResource(mResourceApi, null, Guid.NewGuid(), Guid.NewGuid());
            
            mResourceApi.LoadComplexSemanticResource(resorceToLoad);
            if (resorceToLoad.Uploaded)
            {
                cargasAutores++;
            }
            else
            {
                errorAutores++;
            }
            
        }
        //Console.WriteLine("CARGA MASIVA TERMINADA");
        Console.WriteLine($"Autores cargados correctamente: {cargasAutores}\nAutores fallidos = {errorAutores}\nObras cargados correctamente: {cargaObras}\nAutores cargados correctamente: {errorObras}\n");
    }
    #region Obras
    private static List<string> comprobarObras(string[] nombreObras, JArray obrasArray, ResourceApi mResourceApi)
    {
        List<string> result = new List<string>();
        #region Obtener datos de obras
        foreach (string obraUri in nombreObras)
        {
            // Datos de las obras
            var datosObra = obrasArray.Where(x => x["obraDestacadaURI"].ToString().Equals(obraUri)).Select(x => x).First();
            string titulo = datosObra["nombreObra"].ToString().Trim().Replace("'", " ");
            string uri = getURIObra(titulo, mResourceApi);
            if (uri.Length != 0) { result.Add(uri); continue; }
            uri = cargarObra(datosObra, mResourceApi);

            result.Add(uri);
        }
        #endregion
        return result;
    }
    private static string getURIObra(string titulo, ResourceApi mResourceApi)
    {
        string uri = string.Empty;
        string pOntology = "obraleo";
        string select = string.Empty, where = string.Empty;
        select += $@"SELECT DISTINCT ?s";
        where += $@" WHERE {{ ";
        where += $@"?s ?p '{titulo}'";
        where += $@"}}";

        SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select, where, pOntology);
        //Si está ya en el grafo, obtengo la URI
        if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0 && resultadoQuery.results.bindings.FirstOrDefault()?.Keys.Count > 0)
        {
            foreach (var item in resultadoQuery.results.bindings)
            {
                uri = item["s"].value;
            }
        }
        return uri;
    }
    private static string cargarObra(dynamic datosObra, ResourceApi mResourceApi)
    {
        string titulo = datosObra["nombreObra"].ToString().Trim().Replace("'", " ");
        string fechaValue = datosObra["fechaPublicacion"]?.ToString();
        DateTime fecha = DateTime.Now;
        DateTime.TryParse(fechaValue, out fecha);
        string[] idioma = datosObra["idoma"]?.ToString().Split(" || ");
        string[] generosObra = datosObra["generos"].ToString().Split(" || ");
        Book obra = new();
        if (!string.IsNullOrEmpty(generosObra[0]))
        {
            List<string> generos = comprobarGeneros(generosObra, mResourceApi);
            obra.IdsSchema_genre = generos;
        }

        obra.Schema_name = titulo;
        obra.Schema_datePublished = fecha;
        obra.Schema_inLanguage = new List<string>(idioma);

        // TODO: Llamada al api para la insercion de recursos
        mResourceApi.ChangeOntology("obraleo.owl");
        ComplexOntologyResource resourceLoad = obra.ToGnossApiResource(mResourceApi, null, Guid.NewGuid(), Guid.NewGuid());
        mResourceApi.LoadComplexSemanticResource(resourceLoad);

        if (resourceLoad.Uploaded)
        {
            cargaObras++;
        }
        else
        {
            errorObras++;
        }
        return getURIObra(titulo, mResourceApi);
    }
    #endregion
    #region generos
    private static List<string> comprobarGeneros(string[] generos, ResourceApi mResourceApi)
    {
        List<string> result = new List<string>();
        foreach (string genero in generos)
        {
            string uri = getUriGenero(genero, mResourceApi);
            if (!string.IsNullOrEmpty(uri)) { result.Add(uri); continue; }
            uri = cargarGenero(genero, mResourceApi);
            result.Add(uri);
        }
        return result;
    }

    private static string getUriGenero(string genero, ResourceApi mResourceApi)
    {
        string uri = string.Empty;

        //Obtención del id de la persona cargada en la comunidad
        string pOntology = "genreleo";
        string select = string.Empty, where = string.Empty;
        select += $@"SELECT DISTINCT ?s";
        where += $@" WHERE {{ ";
        where += $@"?s ?p '{genero}'.";
        where += $@"}}";

        SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select, where, pOntology);
        //Si está ya en el grafo, obtengo la URI
        if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0 && resultadoQuery.results.bindings.FirstOrDefault()?.Keys.Count > 0)
        {
            foreach (var item in resultadoQuery.results.bindings)
            {
                uri = item["s"].value;
            }
        }
        return uri;
    }

    private static string cargarGenero(string nomGenero, ResourceApi mResourceApi)
    {
        string identifier = Guid.NewGuid().ToString();

        Genre genero = new(identifier);
        genero.Schema_name = nomGenero;
        mResourceApi.ChangeOntology("genreleo.owl");
        SecondaryResource generoSR = genero.ToGnossApiResource(mResourceApi, $"Genre_{identifier}");
        string mensajeFalloCarga = $"Error en la carga del Género con identificador {identifier} -> Nombre: {genero.Schema_name}";
        try
        {
            mResourceApi.LoadSecondaryResource(generoSR);
            if (!generoSR.Uploaded)
            {
                mResourceApi.Log.Error(mensajeFalloCarga);
            }
        }
        catch (Exception)
        {
            mResourceApi.Log.Error($"Exception -> {mensajeFalloCarga}");
        }
        return getUriGenero(nomGenero, mResourceApi);
    }
    #endregion


}