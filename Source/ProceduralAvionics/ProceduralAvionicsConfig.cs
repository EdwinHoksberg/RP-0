﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RP0.ProceduralAvionics
{
	[Serializable]
	class ProceduralAvionicsConfig : IConfigNode
	{
		[Persistent]
		public string name;

		[Persistent]
		public float tonnageToMassRatio = 10; // default is 10 tons of control to one ton of part mass.  A higher number is more efficient.

		[Persistent]
		public string currentTechNodeName;

		public ProceduralAvionicsTechNode CurrentTechNode
		{
			get
			{
				return TechNodes[currentTechNodeName];
			}
		}	

		public byte[] techNodesSerialized;

		private Dictionary<String, ProceduralAvionicsTechNode> techNodes;
		public Dictionary<String, ProceduralAvionicsTechNode> TechNodes
		{
			get
			{
				if (techNodes == null)
				{
					InitializeTechNodes();
				}
				return techNodes;
			}
		}

		public void Load(ConfigNode node)
		{
			ProceduralAvionicsUtils.Log("Loading Config nodes");
			ConfigNode.LoadObjectFromConfig(this, node);
			techNodes = new Dictionary<string, ProceduralAvionicsTechNode>();
			if (name == null)
			{
				name = node.GetValue("name");
			}
			if (node.HasNode("TECHLIMIT"))
			{
				foreach (ConfigNode tNode in node.GetNodes("TECHLIMIT"))
				{
					ProceduralAvionicsTechNode techNode = new ProceduralAvionicsTechNode();
					techNode.Load(tNode);
					techNodes.Add(techNode.name, techNode);
					ProceduralAvionicsUtils.Log("Loaded TechNode: " + techNode.name);
				}

				List<ProceduralAvionicsTechNode> techNodeList = techNodes.Values.ToList();
				techNodesSerialized = ObjectSerializer.Serialize(techNodeList);
				ProceduralAvionicsUtils.Log("Serialized TechNodes");
			}
			else
			{
				ProceduralAvionicsUtils.Log("No technodes found for " + name);
			}
		}

		public void Save(ConfigNode node)
		{
			ConfigNode.CreateConfigFromObject(this, node);
		}

		public void InitializeTechNodes()
		{
			ProceduralAvionicsUtils.Log("TechNode deserialization needed");
			techNodes = new Dictionary<string, ProceduralAvionicsTechNode>();
			List<ProceduralAvionicsTechNode> techNodeList = ObjectSerializer.Deserialize<List<ProceduralAvionicsTechNode>>(techNodesSerialized);
			foreach (var item in techNodeList)
			{
				ProceduralAvionicsUtils.Log("Deserialized " + item.name);
				techNodes.Add(item.name, item);
			}
			ProceduralAvionicsUtils.Log("Deserialized " + techNodes.Count + " techNodes");
		}
	}
}
