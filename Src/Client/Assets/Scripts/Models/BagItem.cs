using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Models
{
	[StructLayout(LayoutKind.Sequential,Pack = 1)]
	public struct BagItem
	{
		public ushort ItemID;
		public ushort Count;
		public static BagItem zero=new BagItem { ItemID = 0, Count = 0 };
		public BagItem(int itemID,int count)
        {
			this.ItemID=(ushort)itemID;
			this.Count=(ushort)count;
        }
		public static bool operator ==(BagItem lhs, BagItem rhs)
        {
			return lhs.ItemID==rhs.ItemID && lhs.Count==rhs.Count;
        }
		public static bool operator !=(BagItem lhs, BagItem rhs)
        {
			return lhs.ItemID!=rhs.ItemID || lhs.Count!=rhs.Count;
        }
        public override bool Equals(object obj)
        {
            if(obj is BagItem)
            {
                return Equals((BagItem)obj);
            }
            return false;
        }
        public bool Equals(BagItem other)
        {
            return other == this;
        }
        public override int GetHashCode()
        {
            return ItemID.GetHashCode() ^ (Count.GetHashCode() << 2);
        }
        public override string ToString()
        {
            return string.Format("ItemID:{0}, Count:{1}", this.ItemID, this.Count);
        }
    }
}

