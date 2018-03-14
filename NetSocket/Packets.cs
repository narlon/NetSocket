using System;
using System.IO;

namespace JLM.NetSocket
{
    public class PacketBase
    {
        public virtual int PackRealId { get; }
        public virtual byte[] Data { get; }
    }

    public class PacketManager
    {
        public static PacketBase GetPacket(byte[] datas)
        {
            if (datas.Length <= 8) //size = 4, id = 4
                return new PacketBase();

            var packId = (uint)(datas[4] | datas[5] << 8 | datas[6] << 16 | datas[7] << 24);
            byte[] newData = new byte[datas.Length - 4];
            Buffer.BlockCopy(datas, 4, newData, 0, newData.Length);

            switch (packId)
            {
                case PacketLogin.PackId: return new PacketLogin(newData);
            }
            return new PacketBase();
        }
    }

    public class PacketLogin : PacketBase
    {
        public const int PackId = 100001;
        public string Name;

        public override int PackRealId { get { return PackId; } }

        private PacketLogin() { }

        public PacketLogin(string name) : this()
        {
            Name = name;
        }

        public PacketLogin(byte[] bts) : this()
        {
            TBinaryReader sr = new TBinaryReader(bts);
            sr.ReadInt32(); //包id
            Name = sr.ReadString();
        }

        public override byte[] Data
        {
            get
            {
                TBinaryWriter sw = new TBinaryWriter();
                sw.Write(PackId);
                sw.Write(Name);
                return sw.GetBytes();
            }
        }
    }
}