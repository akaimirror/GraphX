﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowcaseApp.WPF.Models
{
    public static class ShowcaseHelper
    {
        static ShowcaseHelper()
        {
            GenerateData(DATASOURCE_SIZE);
        }
    
        /// <summary>
        /// General data source
        /// </summary>
        public static List<DataItem> DataSource { get; set; }

        public const int DATASOURCE_SIZE = 5000;
        public static readonly Random Rand = new Random();

        private static void GenerateData(int count)
        {
            var list = new List<DataItem>();
            for (var i = 0; i < count; i++)
            {
                list.Add(new DataItem { ID = i, Text = "Vertex " + i });
            }
            DataSource = list;
        }

        #region GenerateDataGraph
        /// <summary>
        /// Generate example graph data
        /// </summary>
        /// <param name="count">Items count</param>
        public static GraphExample GenerateDataGraph(int count)
        {
            var graph = new GraphExample();

            foreach (var item in DataSource.Take(count))
                graph.AddVertex(new DataVertex(item.Text) { ID = item.ID, ImageId = Rand.Next(0,3)});

            var vlist = graph.Vertices.ToList();
            var cnt = 1;
            foreach (var item in vlist)
            {
                if (Rand.Next(0, 50) > 25) continue;
                var vertex2 = vlist[Rand.Next(0, graph.VertexCount - 1)];
                var txt = string.Format("{0} -> {1}", item.Text, vertex2.Text);
                graph.AddEdge(new DataEdge(item, vertex2, Rand.Next(1, 50)) { ID = cnt, Text = txt, ToolTipText = txt });
                cnt++;
            }
            return graph;
        }
        #endregion
    }
}
