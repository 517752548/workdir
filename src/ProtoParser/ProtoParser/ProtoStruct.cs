using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoParser
{
    public class ProtoStruct
    {
        public ProtoStruct(string name)
        {
            Name = name;
            Fields = new List<ProtoField>();
        }
        public List<ProtoField> Fields { set; get; }
        public string Name { set; get; }
        public void AddField(ProtoField field)
        {
            Fields.Add(field);
        }

    }
}
