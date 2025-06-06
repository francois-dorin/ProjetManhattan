﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;
using Unity;

namespace ProjetManhattan.Traitements
{
    internal class TraitementTempsRequete : BaseTraitementParLigne, ITraitement
    {       
        public string Name=> "TempsRequete";
        public TraitementTempsRequete(BaseConfig config, IUnityContainer container) : base (container)
        {
            
        }
        protected override void FillRecord(Record[] records, LigneDeLog ligne)
        {
            JObject jsonDescription = new JObject();
            jsonDescription.Add("ip", ligne.IpClient);

            foreach (Record record in records)
            {
                record.Traitement = "TempsRequete";
                record.Target = ligne.CsUriStem;
                record.PropertyName = "TimeTaken";
                record.Value = ligne.TimeTaken.ToString();
                record.Description = jsonDescription.ToString();
                record.Date = ligne.DateHeure;
            }
        }

        public void InitialisationConfig(BaseConfig config)
        {
            ConfigTempsRequetes c = config.GetConfigTraitement<ConfigTempsRequetes>(nameof(TraitementTempsRequete));
            this.Filtre = new IgnoreFastRequest(c.SeuilAlerteTempsRequetes);
        }
    }
}
