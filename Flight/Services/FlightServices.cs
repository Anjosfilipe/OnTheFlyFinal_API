using ClassLibrary;
using Flights.Utils;
using MongoDB.Driver;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Flights.Services
{
    public class FlightServices
    {
        private readonly IMongoCollection<Flight> _flight;

        public FlightServices(IDataBaseSettings settings)
        {
            var flights = new MongoClient(settings.ConnectionString);
            var database = flights.GetDatabase(settings.FlightDataBaseName);
            _flight = database.GetCollection<Flight>(settings.FlightCollectionName);
        }
        public Flight CreateFlights(Flight flight)
        {
            _flight.InsertOne(flight);
            return flight;
        }
        public List<Flight> GetAllFlights() => _flight.Find(flights => true).ToList();
        public Flight GetFlights(string iata, DateTime dateTime) => _flight.Find<Flight>(flights => flights.Departure == dateTime && flights.Destiny.IATA == iata).FirstOrDefault();

        public void UpdateFlights(Flight fligthsIn)
        {
            _flight.ReplaceOne(flights => flights.Departure == fligthsIn.Departure && flights.Destiny.IATA == fligthsIn.Destiny.IATA, fligthsIn);
        }

        public Aircraft GetAircraft(string rab)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44388/api/Aircraft/" + rab); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Aircraft>(message);

        }

        public Airport GetAirport(string iata)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44386/api/Airport/" + iata); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Airport>(message);

        }


        public Company GetCompany(string cnpj)
        {

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("/", "%2F");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44308/api/Company/" + cnpj); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Company>(message);

        }
    }
}
