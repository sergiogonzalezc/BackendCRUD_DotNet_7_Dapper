using System.Runtime.Serialization;

namespace BackendCRUD.Minimal.Api.Model
{
    public abstract class GenericBaseModel<T> : BaseModel
    {
        public T Data
        {
            get;
            set;
        }

        public List<T> DataList
        {
            get;
            set;
        }
    }
}

