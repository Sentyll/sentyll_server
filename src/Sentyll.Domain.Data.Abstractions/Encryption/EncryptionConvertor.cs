using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Sentyll.Domain.Data.Abstractions.Encryption;

public class EncryptionConvertor : ValueConverter<string, string>
{
    public EncryptionConvertor(ConverterMappingHints mappingHints = null)
        : base(x => EncryptionExtension.Encrypt(x), x => EncryptionExtension.Decrypt(x), mappingHints)
    { }
}