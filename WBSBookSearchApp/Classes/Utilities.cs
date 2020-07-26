﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDS.RDF.Query;

namespace WBSBookSearchApp.Classes
{
    public class Utilities
    {
        //Креираме неколку SPARQL queries
        public static string QueryUserSearchBookName(string userSearch)
        {
            string query =
                "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                "PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#> " +
                "PREFIX ontology: <http://dbpedia.org/ontology/> " +
                "SELECT distinct ?s ?bookName ?authorLink ?author  WHERE { " +
                    "?s rdf:type ontology:Book; " +
                       "rdfs:label ?bookName; " +
                       "ontology:author ?authorLink. " +
                    "?authorLink rdfs:label ?author " +

                    "FILTER ( regex (str(?bookName), '" + userSearch + "', 'i') ). " +
                    "FILTER (lang(?author) = 'en') " +
                    "FILTER (lang(?bookName) = 'en') " +
                "} ";

            return query;
        }

        public static string QueryAuthorBooks(string authorLink)
        {
            String query =
                "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                "PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#> " +
                "PREFIX ontology: <http://dbpedia.org/ontology/> " +
                "SELECT distinct ?authorLink ?bookLink ?bookName ?bookAbstract (MAX(?numberOfPages) as ?numberOfPages) ?comment WHERE { " +
                    "?bookLink rdf:type ontology:Book; " +
                        "rdfs:label ?bookName; " +
                        "dbo:abstract ?bookAbstract; " +
                        "rdfs:comment ?comment; " +
                        "ontology:author ?authorLink. " +
                        "OPTIONAL {?bookLink dbo:numberOfPages ?numberOfPages.}" +
                    "FILTER(regex (str(?authorLink), '" + authorLink + "' , 'i') ). " +
                    "FILTER(lang(?bookName) = 'en') " +
                    "FILTER(lang(?bookAbstract) = 'en') " +
                    "FILTER(lang(?comment) = 'en') " +
                "}";
            return query;
        }

        public static string QueryAuthorDetails(string authorLink)
        {
            String query =
                "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                "PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#> " +
                "PREFIX ontology: <http://dbpedia.org/ontology/> " +
                "SELECT distinct ?authorLink ?authorName ?placeOfBirthLink ?PlaceOfBirth ?latitude ?longitude WHERE { " +
                    "?authorLink a dbo:Person; " +
                        "rdfs:label ?authorName; " +
                        "dbo:birthPlace ?placeOfBirthLink. " +
                    "?placeOfBirthLink rdfs:label ?PlaceOfBirth; " +
                        "geo:lat ?latitude; " +
                        "geo:long ?longitude. " +

                    "FILTER(regex (str(?authorLink), '" + authorLink + "', 'i') ). " +
                    "FILTER(lang(?authorName) = 'en') " +
                    "FILTER(lang(?PlaceOfBirth) = 'en') " +
                "}";
            return query;
        }

        //Метод за Query на Dbpedia кој враќа Sparql множество на резултати
        public static SparqlResultSet QueryDbpedia(string query)
        {
            //Тука се дефинира Sparql Endpoint/DPedia Endpoint
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"), "http://dbpedia.org");

            //Тука се прави SELECT query на Endpoint-от
            SparqlResultSet results = endpoint.QueryWithResultSet(query);

            return results;
        }

        //Метод за бришење на @en на крајот од string
        public static string RemoveLast3Cahracters(string word)
        {
            string result = String.Empty;

            if (word.Length > 3)
            {
                result = word.Substring(0, word.Length - 3);
            }

            return result;
        }

        //Броевите добиени од DPedia не се чисти броеви туку линкови. Овој метод служи за бришење на линкот.
        public static string NumberConverter(string word)
        {
            string result = String.Empty;

            if (!string.IsNullOrEmpty(word))
            {
                int index = word.IndexOf("^", StringComparison.Ordinal);
                result = word.Substring(0, index);
            }

            return result;
        }
    }
}