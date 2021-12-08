using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviewConsole.Interface
{
    interface IImporter
    {
        const string CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=moview;AccountKey=/fcjO9BwzZngqDvUGF+Io7O/hXqfC4+MQetDxe27Gxi8CbEUymi1QtIbn31/mlA+AKS0BSWBaVzhIBlGtzjjug==;EndpointSuffix=core.windows.net";
        const string CONTAINER_NAME = "moview";
        void RetrieveFile(int sector);

    }
}
