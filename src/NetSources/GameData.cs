/************************************************/
//本代码自动生成，切勿手动修改
/************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Proto
{
    public enum ArmyCamp
    {
        /// <summary>
        /// 
        /// </summary>
        Player=1,
        /// <summary>
        /// 
        /// </summary>
        Monster=2,

    }
    public class Session : Proto.ISerializerable
    {
        public Session()
        {
            UserName = string.Empty;

        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Time { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            UserName = Encoding.UTF8.GetString(reader.ReadBytes( reader.ReadInt32()));
            Time = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            var UserName_bytes = Encoding.UTF8.GetBytes(UserName);writer.Write(UserName_bytes.Length);writer.Write(UserName_bytes);
            writer.Write(Time);
            
        }

    }
    public class Item : Proto.ISerializerable
    {
        public Item()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public int Num { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Entry { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            Num = reader.ReadInt32();
            Entry = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Num);
            writer.Write(Entry);
            
        }

    }
    public class ItemPackage : Proto.ISerializerable
    {
        public ItemPackage()
        {
            Items = new List<Item>();

        }
        /// <summary>
        /// 
        /// </summary>
        public int CountMax { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<Item> Items { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            CountMax = reader.ReadInt32();
            int Items_Len = reader.ReadInt32();
            while(Items_Len-->0)
            {
                Item Items_Temp = new Item();
                Items_Temp = new Item();Items_Temp.ParseFormBinary(reader);
                Items.Add(Items_Temp );
            }
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(CountMax);
            writer.Write(Items.Count);
            foreach(var i in Items)
            {
                i.ToBinary(writer);               
            }
            
        }

    }
    public class Soldier : Proto.ISerializerable
    {
        public Soldier()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public int ConfigID { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int Num { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            ConfigID = reader.ReadInt32();
            Num = reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(ConfigID);
            writer.Write(Num);
            
        }

    }
    public class Army : Proto.ISerializerable
    {
        public Army()
        {
            Soldiers = new List<Soldier>();

        }
        /// <summary>
        /// 
        /// </summary>
        public List<Soldier> Soldiers { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public ArmyCamp Camp { set; get; }

        public void ParseFormBinary(BinaryReader reader)
        {
            int Soldiers_Len = reader.ReadInt32();
            while(Soldiers_Len-->0)
            {
                Soldier Soldiers_Temp = new Soldier();
                Soldiers_Temp = new Soldier();Soldiers_Temp.ParseFormBinary(reader);
                Soldiers.Add(Soldiers_Temp );
            }
            Camp = (ArmyCamp)reader.ReadInt32();
             
        }

        public void ToBinary(BinaryWriter writer)
        {
            writer.Write(Soldiers.Count);
            foreach(var i in Soldiers)
            {
                i.ToBinary(writer);               
            }
            writer.Write((int)Camp);
            
        }

    }
}