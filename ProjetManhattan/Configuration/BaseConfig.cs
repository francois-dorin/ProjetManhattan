﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Configuration
{
    public class BaseConfig
    {
        public string CheminFichierLog { get; init; }
        public string ConnectionString { get; init; }
        public string ConnectionStringIPLocator { get; init; }
        public  JObject _jConfig { get; init; }
        public DateTime DateTraitement { get; set; }
        public BaseConfig(string filename)
        {
            string json = File.ReadAllText(filename);
            _jConfig = JObject.Parse(json);
            CheminFichierLog = (string)_jConfig["sources"]["fichierDeLogIIS"]["cheminFichierLog"];
            ConnectionString = (string)_jConfig["sources"]["accesBDD"]["connectionString"];
            ConnectionStringIPLocator = (string)_jConfig["sources"]["accesBDD"]["connectionStringIPLocator"];
        }

        public T GetConfigTraitement<T>(string nomTraitement) where T : class
        {
            string nomTraitementEnCamelCase = TransformToCamelCase(nomTraitement);
            if (_jConfig["traitements"][nomTraitementEnCamelCase] != null)
            {
                    return _jConfig["traitements"][nomTraitementEnCamelCase].ToObject<T>();
            } 
            else
            {
                throw new ConfigException($"Le fichier de Config n'est pas valide pour {nomTraitementEnCamelCase}");
            }
        }
        private string TransformToCamelCase(string nomTraitement)
        {
            char majusculeFirst = Char.ToLower(nomTraitement[0]);
            return majusculeFirst + nomTraitement.Substring(1);
        }
    }
}
