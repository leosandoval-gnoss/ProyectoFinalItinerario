using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.Model;
using Gnoss.ApiWrapper.Helpers;
using GnossBase;
using Es.Riam.Gnoss.Web.MVC.Models;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using Gnoss.ApiWrapper.Exceptions;
using System.Diagnostics.CodeAnalysis;
using Genre = GenreleoOntology.Genre;
using Person = EscritorleoOntology.Person;

namespace ObraleoOntology
{
	[ExcludeFromCodeCoverage]
	public class Book : GnossOCBase
	{
		public Book() : base() { } 

		public Book(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			GNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			Schema_genre = new List<Genre>();
			SemanticPropertyModel propSchema_genre = pSemCmsModel.GetPropertyByPath("http://schema.org/genre");
			if(propSchema_genre != null && propSchema_genre.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_genre.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Genre schema_genre = new Genre(propValue.RelatedEntity,idiomaUsuario);
						Schema_genre.Add(schema_genre);
					}
				}
			}
			Schema_author = new List<Person>();
			SemanticPropertyModel propSchema_author = pSemCmsModel.GetPropertyByPath("http://schema.org/author");
			if(propSchema_author != null && propSchema_author.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_author.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person schema_author = new Person(propValue.RelatedEntity,idiomaUsuario);
						Schema_author.Add(schema_author);
					}
				}
			}
			this.Dc_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/title"));
			this.Schema_dateCreated = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/dateCreated"));
			SemanticPropertyModel propSchema_inLanguage = pSemCmsModel.GetPropertyByPath("http://schema.org/inLanguage");
			this.Schema_inLanguage = new List<string>();
			if (propSchema_inLanguage != null && propSchema_inLanguage.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_inLanguage.PropertyValues)
				{
					this.Schema_inLanguage.Add(propValue.Value);
				}
			}
		}

		public Book(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			Schema_genre = new List<Genre>();
			SemanticPropertyModel propSchema_genre = pSemCmsModel.GetPropertyByPath("http://schema.org/genre");
			if(propSchema_genre != null && propSchema_genre.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_genre.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Genre schema_genre = new Genre(propValue.RelatedEntity,idiomaUsuario);
						Schema_genre.Add(schema_genre);
					}
				}
			}
			Schema_author = new List<Person>();
			SemanticPropertyModel propSchema_author = pSemCmsModel.GetPropertyByPath("http://schema.org/author");
			if(propSchema_author != null && propSchema_author.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_author.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person schema_author = new Person(propValue.RelatedEntity,idiomaUsuario);
						Schema_author.Add(schema_author);
					}
				}
			}
			this.Dc_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/title"));
			this.Schema_dateCreated = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/dateCreated"));
			SemanticPropertyModel propSchema_inLanguage = pSemCmsModel.GetPropertyByPath("http://schema.org/inLanguage");
			this.Schema_inLanguage = new List<string>();
			if (propSchema_inLanguage != null && propSchema_inLanguage.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_inLanguage.PropertyValues)
				{
					this.Schema_inLanguage.Add(propValue.Value);
				}
			}
		}

		public virtual string RdfType { get { return "http://schema.org/Book"; } }
		public virtual string RdfsLabel { get { return "http://schema.org/Book"; } }
		[LABEL(LanguageEnum.es,"Género")]
		[RDFProperty("http://schema.org/genre")]
		public  List<Genre> Schema_genre { get; set;}
		public List<string> IdsSchema_genre { get; set;}

		[LABEL(LanguageEnum.es,"Autor")]
		[RDFProperty("http://schema.org/author")]
		public  List<Person> Schema_author { get; set;}
		public List<string> IdsSchema_author { get; set;}

		[LABEL(LanguageEnum.es,"Titulo")]
		[RDFProperty("http://purl.org/dc/elements/1.1/title")]
		public  string Dc_title { get; set;}

		[LABEL(LanguageEnum.es,"Fecha de publicacion")]
		[RDFProperty("http://schema.org/dateCreated")]
		public  DateTime? Schema_dateCreated { get; set;}

		[LABEL(LanguageEnum.es,"Idioma")]
		[RDFProperty("http://schema.org/inLanguage")]
		public  List<string> Schema_inLanguage { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("schema:genre", this.IdsSchema_genre));
			propList.Add(new ListStringOntologyProperty("schema:author", this.IdsSchema_author));
			propList.Add(new StringOntologyProperty("dc:title", this.Dc_title));
			if (this.Schema_dateCreated.HasValue){
				propList.Add(new DateOntologyProperty("schema:dateCreated", this.Schema_dateCreated.Value));
				}
			propList.Add(new ListStringOntologyProperty("schema:inLanguage", this.Schema_inLanguage));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 
		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI)
		{
			return ToGnossApiResource(resourceAPI, new List<string>());
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<Guid> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, null, Guid.Empty, Guid.Empty, listaDeCategorias);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo, List<Guid> listaIdDeCategorias = null)
		{
			ComplexOntologyResource resource = new ComplexOntologyResource();
			Ontology ontology = null;
			GetProperties();
			if(idrecurso.Equals(Guid.Empty) && idarticulo.Equals(Guid.Empty))
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList);
			}
			else{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList,idrecurso,idarticulo);
			}
			resource.Id = GNOSSID;
			resource.Ontology = ontology;
			resource.TextCategories = listaDeCategorias;
			resource.CategoriesIds = listaIdDeCategorias;
			AddResourceTitle(resource);
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Book_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://schema.org/Book>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Book_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://schema.org/Book\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Book_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdsSchema_genre != null)
				{
					foreach(var item2 in this.IdsSchema_genre)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Book_{ResourceID}_{ArticleID}", "http://schema.org/genre", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSchema_author != null)
				{
					foreach(var item2 in this.IdsSchema_author)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Book_{ResourceID}_{ArticleID}", "http://schema.org/author", $"<{item2}>", list, " . ");
					}
				}
				if(this.Dc_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Book_{ResourceID}_{ArticleID}", "http://purl.org/dc/elements/1.1/title",  $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_title)}\"", list, " . ");
				}
				if(this.Schema_dateCreated != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Book_{ResourceID}_{ArticleID}", "http://schema.org/dateCreated",  $"\"{this.Schema_dateCreated.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Schema_inLanguage != null)
				{
					foreach(var item2 in this.Schema_inLanguage)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Book_{ResourceID}_{ArticleID}", "http://schema.org/inLanguage", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"obraleo\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://schema.org/Book\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_title)}\"", list, " . ");
			string search = string.Empty;
				if(this.IdsSchema_genre != null)
				{
					foreach(var item2 in this.IdsSchema_genre)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/genre", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSchema_author != null)
				{
					foreach(var item2 in this.IdsSchema_author)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/author", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Dc_title != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://purl.org/dc/elements/1.1/title",  $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_title)}\"", list, " . ");
				}
				if(this.Schema_dateCreated != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/dateCreated",  $"{this.Schema_dateCreated.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Schema_inLanguage != null)
				{
					foreach(var item2 in this.Schema_inLanguage)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/inLanguage", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
			if (listaSearch != null && listaSearch.Count > 0)
			{
				foreach(string valorSearch in listaSearch)
				{
					search += $"{valorSearch} ";
				}
			}
			if(!string.IsNullOrEmpty(search))
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/search", $"\"{GenerarTextoSinSaltoDeLinea(search.ToLower())}\"", list, " . ");
			}
			return list;
		}

		public override KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI)
		{

			//Insert en la tabla Documento
			string tags = "";
			foreach(string tag in tagList)
			{
				tags += $"{tag}, ";
			}
			if (!string.IsNullOrEmpty(tags))
			{
				tags = tags.Substring(0, tags.LastIndexOf(','));
			}
			string titulo = $"{this.Dc_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/ObraleoOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Dc_title;
		}





	}
}
