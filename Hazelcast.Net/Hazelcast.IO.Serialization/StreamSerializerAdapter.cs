using System.Text;

namespace Hazelcast.IO.Serialization
{
    internal sealed class StreamSerializerAdapter<T> : ISerializerAdapter
    {
        private readonly IStreamSerializer<T> serializer;
        private readonly SerializationService service;

        public StreamSerializerAdapter(SerializationService service, IStreamSerializer<T> serializer)
        {
            this.service = service;
            this.serializer = serializer;
        }

        /// <exception cref="System.IO.IOException"></exception>
        public void Write(IObjectDataOutput output, object obj)
        {
            serializer.Write(output, (T) obj);
        }

        /// <exception cref="System.IO.IOException"></exception>
        public object Read(IObjectDataInput input)
        {
            return serializer.Read(input);
        }

        public int GetTypeId()
        {
            return serializer.GetTypeId();
        }

        public void Destroy()
        {
            serializer.Destroy();
        }

        public ISerializer GetImpl()
        {
            return serializer;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("SerializerAdapter{");
            sb.Append("serializer=").Append(serializer);
            sb.Append('}');
            return sb.ToString();
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (o == null || GetType() != o.GetType())
            {
                return false;
            }
            var that = (StreamSerializerAdapter<T>) o;
            if (serializer != null ? !serializer.Equals(that.serializer) : that.serializer != null)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return serializer != null ? serializer.GetHashCode() : 0;
        }
    }
}