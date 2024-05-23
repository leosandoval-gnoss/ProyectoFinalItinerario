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

namespace EscritorleoOntology
{
	[ExcludeFromCodeCoverage]
	public class Country : GnossOCBase
	{
		public Country() : base() { } 

		public Country(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			mGNOSSID = pSemCmsModel.Entity.Uri;
			mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propSchema_containedInPlace = pSemCmsModel.GetPropertyByPath("http://schema.org/containedInPlace");
			this.Schema_containedInPlace = new List<string>();
			if (propSchema_containedInPlace != null && propSchema_containedInPlace.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSchema_containedInPlace.PropertyValues)
				{
					this.Schema_containedInPlace.Add(propValue.Value);
				}
			}
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://schema.org/name"));
		}

		public virtual string RdfType { get { return "http://schema.org/Country"; } }
		public virtual string RdfsLabel { get { return "http://schema.org/Country"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"Continente")]
		[RDFProperty("http://schema.org/containedInPlace")]
		public  List<string> Schema_containedInPlace { get; set;}

		[LABEL(LanguageEnum.es,"Pais")]
		[RDFProperty("http://schema.org/name")]
		public  string Schema_name { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("schema:containedInPlace", this.Schema_containedInPlace));
			propList.Add(new StringOntologyProperty("schema:name", this.Schema_name));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
