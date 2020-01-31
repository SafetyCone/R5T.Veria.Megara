using System;

using R5T.Bath;
using R5T.Bedford;
using R5T.Megara;
using R5T.Vandalia;


namespace R5T.Veria.Megara
{
    public class RoundTripFileSerializationVerifier<T> : IRoundTripFileSerializationVerifier<T>
    {
        private IHumanOutput HumanOutput { get; }
        private IFileSerializer<T> FileSerializer { get; }
        private IFileEqualityComparer FileEqualityComparer { get; }
        private IValueEqualityComparer<T> ValueEqualityComparer { get; }


        public RoundTripFileSerializationVerifier(
            IHumanOutput humanOutput,
            IFileSerializer<T> fileSerializer,
            IFileEqualityComparer fileEqualityComparer,
            IValueEqualityComparer<T> valueEqualityComparer)
        {
            this.HumanOutput = humanOutput;
            this.FileSerializer = fileSerializer;
            this.FileEqualityComparer = fileEqualityComparer;
            this.ValueEqualityComparer = valueEqualityComparer;
        }

        public bool Verify(T value, string serializationFilePath1, string serializationFilePath2)
        {
            this.FileSerializer.Serialize(serializationFilePath1, value);

            var deserializedValue = this.FileSerializer.Deserialize(serializationFilePath1);

            var valueEquality = this.TestValueEquality(value, deserializedValue);

            this.FileSerializer.Serialize(serializationFilePath2, deserializedValue);

            var fileEquality = this.TestFileEquality(serializationFilePath1, serializationFilePath2);

            var output = valueEquality && fileEquality;
            return output;
        }

        public bool Verify(string sourceFilePath, string serializationFilePath)
        {
            var value = this.FileSerializer.Deserialize(sourceFilePath);

            this.FileSerializer.Serialize(serializationFilePath, value);

            var fileEquality = this.TestFileEquality(sourceFilePath, serializationFilePath);

            var deserializedValue = this.FileSerializer.Deserialize(serializationFilePath);

            var valueEquality = this.TestValueEquality(value, deserializedValue);

            var output = valueEquality && fileEquality;
            return output;
        }

        private bool TestValueEquality(T value1, T value2)
        {
            var valueEquality = this.ValueEqualityComparer.Equals(value1, value2);
            if (!valueEquality)
            {
                this.HumanOutput.WriteLine("Value and deserialized values not equal.");
            }

            return valueEquality;
        }

        private bool TestFileEquality(string filePath1, string filePath2)
        {
            var fileEquality = this.FileEqualityComparer.Equals(filePath1, filePath2);
            if (!fileEquality)
            {
                this.HumanOutput.WriteLine("Serialization files note equal.");
            }

            return fileEquality;
        }
    }
}
