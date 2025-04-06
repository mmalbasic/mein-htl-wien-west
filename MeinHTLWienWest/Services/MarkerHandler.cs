using Dapper;
using NpgsqlTypes;
using System.Data;

namespace MeinHTLWienWest.Services
{
    public class MarkerHandler : SqlMapper.TypeHandler<Marker>
    {
        public override void SetValue(IDbDataParameter parameter, Marker value)
        {
            parameter.Value = value;
        }

        public override Marker Parse(object value)
        {
            NpgsqlPoint np = (NpgsqlPoint)value;
            return new Marker() { X = np.X, Y = np.Y };
        }
    }
}
