using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Astandy.Base
{
    public class GetPlayerRequest : IMessage<GetPlayerRequest>
    {
        // Исправлено: имя свойства должно быть строго Descriptor
        public MessageDescriptor Descriptor => null!;
        public void WriteTo(CodedOutputStream output) { }
        public int CalculateSize() => 0;
        public void MergeFrom(GetPlayerRequest message) { }
        public void MergeFrom(CodedInputStream input) { }
        public bool Equals(GetPlayerRequest? other) => true;
        public GetPlayerRequest Clone() => new GetPlayerRequest();
    }

    public class GetPlayerResponse : IMessage<GetPlayerResponse>
    {
        public static MessageParser<GetPlayerResponse> Parser { get; } = new(() => new GetPlayerResponse());

        // Исправлено: имя свойства должно быть строго Descriptor
        public MessageDescriptor Descriptor => null!;
        public void WriteTo(CodedOutputStream output) { }
        public int CalculateSize() => 0;
        public void MergeFrom(GetPlayerResponse message) { }
        public void MergeFrom(CodedInputStream input) { }
        public bool Equals(GetPlayerResponse? other) => true;
        public GetPlayerResponse Clone() => new GetPlayerResponse();
    }
    public class SubscribeResponse : IMessage<SubscribeResponse>
    {
        public static MessageParser<SubscribeResponse> Parser { get; } = new(() => new SubscribeResponse());
        public MessageDescriptor Descriptor => null!;
        public void WriteTo(CodedOutputStream output) { }
        public int CalculateSize() => 0;
        public void MergeFrom(SubscribeResponse message) { }
        public void MergeFrom(CodedInputStream input) { }
        public bool Equals(SubscribeResponse? other) => true;
        public SubscribeResponse Clone() => new SubscribeResponse();
    }

    public class ADHHEGBDFBEBGGC : IMessage<ADHHEGBDFBEBGGC>
    {
        public static MessageParser<ADHHEGBDFBEBGGC> Parser { get; } = new(() => new ADHHEGBDFBEBGGC());
        public MessageDescriptor Descriptor => null!;
        public void WriteTo(CodedOutputStream output) { }
        public int CalculateSize() => 0;
        public void MergeFrom(ADHHEGBDFBEBGGC message) { }
        public void MergeFrom(CodedInputStream input) { }
        public bool Equals(ADHHEGBDFBEBGGC? other) => true;
        public ADHHEGBDFBEBGGC Clone() => new ADHHEGBDFBEBGGC();
    }
    public class SubscribeRequest : IMessage<SubscribeRequest>
    {
        public MessageDescriptor Descriptor => null!;
        public void WriteTo(CodedOutputStream output) { }
        public int CalculateSize() => 0;
        public void MergeFrom(SubscribeRequest message) { }
        public void MergeFrom(CodedInputStream input) { }
        public bool Equals(SubscribeRequest? other) => true;
        public SubscribeRequest Clone() => new SubscribeRequest();
    }

    public class GACHFBFBBEHDAAD : IMessage<GACHFBFBBEHDAAD>
    {
        public string GFCCADHDDFAFHFE { get; set; } = "";
        public MessageDescriptor Descriptor => null!;
        public void WriteTo(CodedOutputStream output) { }
        public int CalculateSize() => 0;
        public void MergeFrom(GACHFBFBBEHDAAD message) { }
        public void MergeFrom(CodedInputStream input) { }
        public bool Equals(GACHFBFBBEHDAAD? other) => true;
        public GACHFBFBBEHDAAD Clone() => new GACHFBFBBEHDAAD();
    }

    public class CHGACEEHFADEDHH : IMessage<CHGACEEHFADEDHH>
    {
        public ByteString DCAGCDFCHBBDCDB { get; set; } = ByteString.Empty;
        public ByteString GABAFEDDDBEGGAE { get; set; } = ByteString.Empty;
        public ByteString CGCGBGBEGCADABH { get; set; } = ByteString.Empty;
        public MessageDescriptor Descriptor => null!;
        public void WriteTo(CodedOutputStream output) { }
        public int CalculateSize() => 0;
        public void MergeFrom(CHGACEEHFADEDHH message) { }
        public void MergeFrom(CodedInputStream input) { }
        public bool Equals(CHGACEEHFADEDHH? other) => true;
        public CHGACEEHFADEDHH Clone() => new CHGACEEHFADEDHH();
    }

    public class BBFGDBCEGCBFBEE : IMessage<BBFGDBCEGCBFBEE>
    {
        public static MessageParser<BBFGDBCEGCBFBEE> Parser { get; } = new(() => new BBFGDBCEGCBFBEE());
        public ByteString FBHHACGFCHFEEED { get; set; } = ByteString.Empty;
        public MessageDescriptor Descriptor => null!;
        public void WriteTo(CodedOutputStream output) { }
        public int CalculateSize() => 0;
        public void MergeFrom(BBFGDBCEGCBFBEE message) { }
        public void MergeFrom(CodedInputStream input) { }
        public bool Equals(BBFGDBCEGCBFBEE? other) => true;
        public BBFGDBCEGCBFBEE Clone() => new BBFGDBCEGCBFBEE();
    }
}