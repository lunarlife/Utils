using System.Collections;
using System.Reflection;

namespace Utils.Enums
{
    public sealed class Enum<T> : IEnumerable<T> where T : EnumType
    {
        private static MethodInfo _dataMethod =
            typeof(EnumType).GetMethod("SetData", BindingFlags.Instance | BindingFlags.NonPublic)!;

        private readonly bool _modifiable = true;
        private Dictionary<string, T> _members = new();
        public int Count => _members.Count;
        public T[] Values => _members.Values.ToArray();

        public Enum(bool modifiable = true)
        {
            _modifiable = modifiable;
        }
        public T1 AddMember<T1>(string name) where T1 : T, new()
        {
            if (!_modifiable) throw new EnumException("enum is not modifiable");
            if (_members.ContainsKey(name)) throw new EnumException("name is already exists");
            var member = new T1();
            _dataMethod.Invoke(member, new object[] { _members.Count, name });
            _members.Add(name, member);
            return member;
        }

        public T AddMember(string name, T member)
        {
            if (!_modifiable) throw new EnumException("enum is not modifiable");
            if (_members.ContainsKey(name)) throw new EnumException("name is already exists");
            _dataMethod.Invoke(member, new object[] { _members.Count, name });
            _members.Add(name, member);
            return member;
        }

        public void AddRange(Dictionary<string, T> add)
        {
            foreach (var (name, value) in add) AddMember(name, value);
        }
        public void RemoveMember(string name)
        {
            if (!_modifiable) throw new EnumException("enum is not modifiable");
            if (!_members.ContainsKey(name)) throw new EnumException("member not founded");
            _members.Remove(name);
        }

        public T this[string name] => ValueOf(name);
        public T this[int key] => ValueOf(key);

        public T ValueOf(string name)
        {
            if (!_members.ContainsKey(name)) throw new EnumException("invalid enum member");
            return _members[name];
        }


        public T ValueOf(int index)
        {
            if (index > _members.Count - 1) throw new EnumException("invalid enum member");
            return _members.Values.ToArray()[index];
        }

        public bool Contains(T value) => _members.Values.Contains(value);

        public bool Contains(int value)
        {
            return _members.Count > value;
        }
        public bool ContainsAll(params T[] values) => values.All(val => _members.ContainsValue(val));
        public bool ContainsAll(params string[] values) => values.All(val => _members.ContainsKey(val));
        public bool Contains(string value) => _members.ContainsKey(value);
        public IEnumerator<T> GetEnumerator() => new EnumEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        public class EnumEnumerator : IEnumerator<T>
        {
            private readonly Enum<T> _enum;
            private int _index = -1;
            public T Current => _enum[_index];

            object IEnumerator.Current => Current;

            public EnumEnumerator(Enum<T> @enum)
            {
                _enum = @enum;
            }

            public bool MoveNext()
            {
                _index++;
                return _index < _enum.Count;
            }

            public void Reset()
            {
                _index = 0;
            }

            public void Dispose()
            {

            }
        }

    }


    public class EnumException : Exception
    {
        public EnumException(string msg) : base(msg)
        {
        }
    }
}