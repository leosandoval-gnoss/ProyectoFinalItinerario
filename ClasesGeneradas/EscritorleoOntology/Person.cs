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
using Award = PremioleoOntology.Award;
using Book = ObraleoOntology.Book;

namespace EscritorleoOntology
{
	[ExcludeFromCodeCoverage]
	public class Person : GnossOCBase
	{
		public Person() : base() { } 

		public Person(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			GNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			Schema_awards = new List<Award>();
			SemanticPropertyModel propSchema_awards = pSemCmsModel.GetPropertyByPath("http://schema.org/awards");
			if(propSchema_awards != null && propSchema_awards.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_awards.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Award schema_awards = new Award(propValue.RelatedEntity,idiomaUsuario);
						Schema_awards.Add(schema_awards);
					}
				}
			}
			Try_authorOf = new List<Book>();
			SemanticPropertyModel propTry_authorOf = pSemCmsModel.GetPropertyByPath("http://try.gnoss.com/ontology#authorOf");
			if(propTry_authorOf != null && propTry_authorOf.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propTry_authorOf.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Book try_authorOf = new Book(propValue.RelatedEntity,idiomaUsuario);
						Try_authorOf.Add(try_authorOf);
					}
				}
			}
			Schema_nationality = new List<Country>();
			SemanticPropertyModel propSchema_nationality = pSemCmsModel.GetPropertyByPath("http://schema.org/nationality");
			if(propSchema_nationality != null && propSchema_nationality.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_nationality.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Country schema_nationality = new Country(propValue.RelatedEntity,idiomaUsuario);
						Schema_nationality.Add(schema_nationality);
					}
				}
			}
			this.Foaf_birthday = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/birthday"));
			SemanticPropertyModel propSchema_occupation = pSemCmsModel.GetPropertyByPath("http://schema.org/occupation");
			this.Schema_occupation = new List<string>();
			if (propSchema_occupation != null && propSchema_occupation.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_occupation.PropertyValues)
				{
					this.Schema_occupation.Add(propValue.Value);
				}
			}
			this.Foaf_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/name"));
			SemanticPropertyModel propSchema_movement = pSemCmsModel.GetPropertyByPath("http://schema.org/movement");
			this.Schema_movement = new List<string>();
			if (propSchema_movement != null && propSchema_movement.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_movement.PropertyValues)
				{
					this.Schema_movement.Add(propValue.Value);
				}
			}
			this.Foaf_gender = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/gender"));
			this.Foaf_img = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/img"));
			SemanticPropertyModel propFoaf_surname = pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/surname");
			this.Foaf_surname = new List<string>();
			if (propFoaf_surname != null && propFoaf_surname.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propFoaf_surname.PropertyValues)
				{
					this.Foaf_surname.Add(propValue.Value);
				}
			}
		}

		public Person(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			Schema_awards = new List<Award>();
			SemanticPropertyModel propSchema_awards = pSemCmsModel.GetPropertyByPath("http://schema.org/awards");
			if(propSchema_awards != null && propSchema_awards.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_awards.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Award schema_awards = new Award(propValue.RelatedEntity,idiomaUsuario);
						Schema_awards.Add(schema_awards);
					}
				}
			}
			Try_authorOf = new List<Book>();
			SemanticPropertyModel propTry_authorOf = pSemCmsModel.GetPropertyByPath("http://try.gnoss.com/ontology#authorOf");
			if(propTry_authorOf != null && propTry_authorOf.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propTry_authorOf.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Book try_authorOf = new Book(propValue.RelatedEntity,idiomaUsuario);
						Try_authorOf.Add(try_authorOf);
					}
				}
			}
			Schema_nationality = new List<Country>();
			SemanticPropertyModel propSchema_nationality = pSemCmsModel.GetPropertyByPath("http://schema.org/nationality");
			if(propSchema_nationality != null && propSchema_nationality.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_nationality.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Country schema_nationality = new Country(propValue.RelatedEntity,idiomaUsuario);
						Schema_nationality.Add(schema_nationality);
					}
				}
			}
			this.Foaf_birthday = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/birthday"));
			SemanticPropertyModel propSchema_occupation = pSemCmsModel.GetPropertyByPath("http://schema.org/occupation");
			this.Schema_occupation = new List<string>();
			if (propSchema_occupation != null && propSchema_occupation.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_occupation.PropertyValues)
				{
					this.Schema_occupation.Add(propValue.Value);
				}
			}
			this.Foaf_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/name"));
			SemanticPropertyModel propSchema_movement = pSemCmsModel.GetPropertyByPath("http://schema.org/movement");
			this.Schema_movement = new List<string>();
			if (propSchema_movement != null && propSchema_movement.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_movement.PropertyValues)
				{
					this.Schema_movement.Add(propValue.Value);
				}
			}
			this.Foaf_gender = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/gender"));
			this.Foaf_img = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/img"));
			SemanticPropertyModel propFoaf_surname = pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/surname");
			this.Foaf_surname = new List<string>();
			if (propFoaf_surname != null && propFoaf_surname.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propFoaf_surname.PropertyValues)
				{
					this.Foaf_surname.Add(propValue.Value);
				}
			}
		}

		public virtual string RdfType { get { return "http://xmlns.com/foaf/0.1/Person"; } }
		public virtual string RdfsLabel { get { return "http://xmlns.com/foaf/0.1/Person"; } }
		[LABEL(LanguageEnum.es,"Premios")]
		[RDFProperty("http://schema.org/awards")]
		public  List<Award> Schema_awards { get; set;}
		public List<string> IdsSchema_awards { get; set;}

		[LABEL(LanguageEnum.es,"Autor de")]
		[RDFProperty("http://try.gnoss.com/ontology#authorOf")]
		public  List<Book> Try_authorOf { get; set;}
		public List<string> IdsTry_authorOf { get; set;}

		[LABEL(LanguageEnum.es,"Nacionalidad")]
		[RDFProperty("http://schema.org/nationality")]
		public  List<Country> Schema_nationality { get; set;}

		[LABEL(LanguageEnum.es,"Fecha de nacimiento")]
		[RDFProperty("http://xmlns.com/foaf/0.1/birthday")]
		public  DateTime? Foaf_birthday { get; set;}

		[LABEL(LanguageEnum.es,"Ocupaciones")]
		[RDFProperty("http://schema.org/occupation")]
		public  List<string> Schema_occupation { get; set;}

		[LABEL(LanguageEnum.es,"Nombre")]
		[RDFProperty("http://xmlns.com/foaf/0.1/name")]
		public  string Foaf_name { get; set;}

		[LABEL(LanguageEnum.es,"Movimientos")]
		[RDFProperty("http://schema.org/movement")]
		public  List<string> Schema_movement { get; set;}

		[LABEL(LanguageEnum.es,"Genero")]
		[RDFProperty("http://xmlns.com/foaf/0.1/gender")]
		public  string Foaf_gender { get; set;}

		[LABEL(LanguageEnum.es,"Foto")]
		[RDFProperty("http://xmlns.com/foaf/0.1/img")]
		public  string Foaf_img { get; set;}

		[LABEL(LanguageEnum.es,"Apellido")]
		[RDFProperty("http://xmlns.com/foaf/0.1/surname")]
		public  List<string> Foaf_surname { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("schema:awards", this.IdsSchema_awards));
			propList.Add(new ListStringOntologyProperty("try:authorOf", this.IdsTry_authorOf));
			if (this.Foaf_birthday.HasValue){
				propList.Add(new DateOntologyProperty("foaf:birthday", this.Foaf_birthday.Value));
				}
			propList.Add(new ListStringOntologyProperty("schema:occupation", this.Schema_occupation));
			propList.Add(new StringOntologyProperty("foaf:name", this.Foaf_name));
			propList.Add(new ListStringOntologyProperty("schema:movement", this.Schema_movement));
			propList.Add(new StringOntologyProperty("foaf:gender", this.Foaf_gender));
			propList.Add(new StringOntologyProperty("foaf:img", this.Foaf_img));
			propList.Add(new ListStringOntologyProperty("foaf:surname", this.Foaf_surname));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Schema_nationality!=null){
				foreach(Country prop in Schema_nationality){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCountry = new OntologyEntity("http://schema.org/Country", "http://schema.org/Country", "schema:nationality", prop.propList, prop.entList);
				entList.Add(entityCountry);
				prop.Entity = entityCountry;
				}
			}
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
			GetEntities();
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
			AddResourceDescription(resource);
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://xmlns.com/foaf/0.1/Person>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://xmlns.com/foaf/0.1/Person\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Schema_nationality != null)
			{
			foreach(var item0 in this.Schema_nationality)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Country_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://schema.org/Country>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Country_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://schema.org/Country\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Country_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://schema.org/nationality", $"<{resourceAPI.GraphsUrl}items/Country_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Schema_containedInPlace != null)
				{
					foreach(var item2 in item0.Schema_containedInPlace)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Country_{ResourceID}_{item0.ArticleID}", "http://schema.org/containedInPlace", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(item0.Schema_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Country_{ResourceID}_{item0.ArticleID}", "http://schema.org/name",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_name)}\"", list, " . ");
				}
			}
			}
				if(this.IdsSchema_awards != null)
				{
					foreach(var item2 in this.IdsSchema_awards)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://schema.org/awards", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsTry_authorOf != null)
				{
					foreach(var item2 in this.IdsTry_authorOf)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://try.gnoss.com/ontology#authorOf", $"<{item2}>", list, " . ");
					}
				}
				if(this.Foaf_birthday != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://xmlns.com/foaf/0.1/birthday",  $"\"{this.Foaf_birthday.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Schema_occupation != null)
				{
					foreach(var item2 in this.Schema_occupation)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://schema.org/occupation", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Foaf_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://xmlns.com/foaf/0.1/name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name)}\"", list, " . ");
				}
				if(this.Schema_movement != null)
				{
					foreach(var item2 in this.Schema_movement)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://schema.org/movement", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Foaf_gender != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://xmlns.com/foaf/0.1/gender",  $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_gender)}\"", list, " . ");
				}
				if(this.Foaf_img != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://xmlns.com/foaf/0.1/img",  $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_img)}\"", list, " . ");
				}
				if(this.Foaf_surname != null)
				{
					foreach(var item2 in this.Foaf_surname)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://xmlns.com/foaf/0.1/surname", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"escritorleo\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://xmlns.com/foaf/0.1/Person\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name)}\"", list, " . ");
			string search = string.Empty;
			if(this.Schema_nationality != null)
			{
			foreach(var item0 in this.Schema_nationality)
			{
				AgregarTripleALista($"http://gnossAuxiliar/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasEntidadAuxiliar", $"<{resourceAPI.GraphsUrl}items/Country_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/nationality", $"<{resourceAPI.GraphsUrl}items/Country_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Schema_containedInPlace != null)
				{
					foreach(var item2 in item0.Schema_containedInPlace)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Country_{ResourceID}_{item0.ArticleID}", "http://schema.org/containedInPlace", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(item0.Schema_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Country_{ResourceID}_{item0.ArticleID}", "http://schema.org/name",  $"\"{GenerarTextoSinSaltoDeLinea(item0.Schema_name)}\"", list, " . ");
				}
			}
			}
				if(this.IdsSchema_awards != null)
				{
					foreach(var item2 in this.IdsSchema_awards)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/awards", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsTry_authorOf != null)
				{
					foreach(var item2 in this.IdsTry_authorOf)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://try.gnoss.com/ontology#authorOf", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Foaf_birthday != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/birthday",  $"{this.Foaf_birthday.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Schema_occupation != null)
				{
					foreach(var item2 in this.Schema_occupation)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/occupation", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Foaf_name != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name)}\"", list, " . ");
				}
				if(this.Schema_movement != null)
				{
					foreach(var item2 in this.Schema_movement)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://schema.org/movement", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Foaf_gender != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/gender",  $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_gender)}\"", list, " . ");
				}
				if(this.Foaf_img != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/img",  $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_img)}\"", list, " . ");
				}
				if(this.Foaf_surname != null)
				{
					foreach(var item2 in this.Foaf_surname)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/surname", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
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
			string titulo = $"{this.Foaf_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Foaf_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/EscritorleoOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Foaf_name;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Foaf_name;
		}




	}
}
