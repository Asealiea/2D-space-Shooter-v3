using System;

namespace RSG.Trellis
{
    public class EmbeddedAttribute : Attribute
    {
        public EmbeddedAttribute(string childName = null)
        {
            ChildName = childName;
        }

        public string ChildName;
    }
}