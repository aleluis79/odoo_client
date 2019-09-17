﻿using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using OdooRpc.CoreCLR.Client;
using OdooRpc.CoreCLR.Client.Interfaces;
using OdooRpc.CoreCLR.Client.Models;
using OdooRpc.CoreCLR.Client.Models.Parameters;

namespace api
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Iniciando...");

            //Creo objeto OdooConnectionInfo con información de conección
            OdooConnectionInfo ConnectionInfo = new OdooConnectionInfo()
            {
                IsSSL = false,
                Host = "localhost",
                Port = 8069,
                Database = "db",
                Username = "admin@admin",
                Password = "1234"
            };

            //Me conecto a Odoo
            IOdooRpcClient RpcClient = new OdooRpcClient(ConnectionInfo);

            //Obtengo la versión de Odoo y la muestro por consola
            var odooVersion = await RpcClient.GetOdooVersion();            
            Console.WriteLine("Odoo Version: {0} - {1}", odooVersion.ServerVersion, odooVersion.ProtocolVersion);

            //Login
            await RpcClient.Authenticate();
            if (RpcClient.SessionInfo.IsLoggedIn)
            {
                Console.WriteLine("Login correcto => Usuario Id: {0}", RpcClient.SessionInfo.UserId);
            }
            else
            {
                Console.WriteLine("Login incorrecto");
            }

            //Obtengo datos de los autos
            try
            {
                var fieldParams = new OdooFieldParameters();
                fieldParams.Add("model_id");
                fieldParams.Add("display_name");                

                var autos = await RpcClient.GetAll<JObject[]>("fleet.vehicle", fieldParams, new OdooPaginationParameters().OrderByDescending("name"));
                foreach (var auto in autos) {
                    Console.WriteLine(auto);
                }
                //Console.WriteLine(autos.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error recuperando los autos desde Odoo: {0}", ex.Message);
            }

        }
    }
}
