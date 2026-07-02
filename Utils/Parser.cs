using System;
using System.Buffers.Binary;
using Google.Protobuf;
using Astandy.Base; // Важно для видимости ClientMsg, ServerMsg и т.д.

namespace Astandy.Utils
{
    public static class Parser
    {
        public static int ParseHeaderLength(ReadOnlySpan<byte> headerBytes)
        {
            return BinaryPrimitives.ReadInt32BigEndian(headerBytes);
        }

        public static byte[] NewMsg(string id, uint code, byte[] payload, string serviceName = "", string methodName = "")
        {
            var msg = new ClientMsg
            {
                Id = id,
                Code = code,
                Cls = serviceName,
                Func = methodName
            };

            if (payload != null && payload.Length > 0)
            {
                var binaryValue = new BinaryValue
                {
                    One = ByteString.CopyFrom(payload)
                };
                msg.Data.Add(binaryValue);
            }

            return msg.ToByteArray();
        }

        public static ServerMsg ParseResponse(byte[] data)
        {
            return ServerMsg.Parser.ParseFrom(data);
        }
    }
}