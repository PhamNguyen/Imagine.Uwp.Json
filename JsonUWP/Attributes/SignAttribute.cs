using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonUWP.Attributes
{
    public class SignAttribute:Attribute
    {
        public String Name { get; set; }

        private Type type;

        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        private bool isList = false;

        public bool IsList
        {
            get { return isList; }
            set { isList = value; }
        }


        public SignAttribute(String name) {
            this.Name = name;
        }
    }
}
