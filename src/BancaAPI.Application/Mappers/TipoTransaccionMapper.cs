// src/BancaAPI.Application/Mappers/TipoTransaccionMapper.cs

using BancaAPI.Domain.Constants;
using BancaAPI.Domain.Enums;

namespace BancaAPI.Application.Mappers
{
    public static class TipoTransaccionMapper
    {
        public static Guid ObtenerId(TipoTransaccionEnum tipo)
        {
            return tipo switch
            {
                TipoTransaccionEnum.Deposito => TipoTransaccionConst.Deposito,
                TipoTransaccionEnum.Retiro => TipoTransaccionConst.Retiro,
                _ => throw new ArgumentOutOfRangeException(nameof(tipo), "Tipo inválido")
            };
        }
    }
}
