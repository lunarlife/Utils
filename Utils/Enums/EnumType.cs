namespace Utils.Enums
{
    
    public abstract class EnumType : IEquatable<EnumType>
    {
        public int ID { get; private set; }
        public string Name { get; private set; } = "";

        private void SetData(int id, string name)
        {
            ID = id; 
            Name = name;
        }


        public static implicit operator int(EnumType type) => type.ID;
        public static implicit operator string(EnumType type) => type.Name;

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(EnumType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EnumType)obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}