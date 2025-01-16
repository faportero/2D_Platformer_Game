using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modelos
{
    //[Serializable]
    [Serializable]
    public class RankingModel
    {
        public RankingModel()
        {
        }
        public RankingModel(int error, string msg, int rowCount, List<UsuarioRankingModel> lista)
        {
            this.error = error;
            this.msg = msg;
            this.rowCount = rowCount;
            this.lista = lista;
        }

        public int error;
        public string msg;
        public int rowCount;
        public List<UsuarioRankingModel> lista;

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
