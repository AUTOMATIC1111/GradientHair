using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Verse;

namespace GradientHair
{
    public class PatchOperationAddComp : PatchOperationPathed
    {
        // Token: 0x060069CE RID: 27086 RVA: 0x00232584 File Offset: 0x00230784
        protected override bool ApplyWorker(XmlDocument xml)
        {
            XmlNode node = value.node;
            bool result = false;
            IEnumerator enumerator = xml.SelectNodes(xpath).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    XmlNode xmlNode = obj as XmlNode;
                    XmlNode xmlNode2 = xmlNode["comps"];
                    if (xmlNode2 == null)
                    {
                        xmlNode2 = xmlNode.OwnerDocument.CreateElement("comps");
                        xmlNode.AppendChild(xmlNode2);
                    }
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        xmlNode2.AppendChild(xmlNode.OwnerDocument.ImportNode(node.ChildNodes[i], true));
                    }
                    result = true;
                }
            }
            finally
            {
                IDisposable disposable;
                if ((disposable = (enumerator as IDisposable)) != null)
                {
                    disposable.Dispose();
                }
            }
            return result;
        }

        public XmlContainer value;
    }
}
