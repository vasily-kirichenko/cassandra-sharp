﻿// cassandra-sharp - high performance .NET driver for Apache Cassandra
// Copyright (c) 2011-2013 Pierre Chalamet
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace CassandraSharp.CQLBinaryProtocol.Queries
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CassandraSharp.Exceptions;
    using CassandraSharp.Extensibility;
    using CassandraSharp.Utils.Stream;

    internal class CreateOptionsQuery : Query<Dictionary<string, string[]>>
    {
        public CreateOptionsQuery(IConnection connection)
                : base(connection)
        {
        }

        protected override IEnumerable<Dictionary<string, string[]>> CreateReader(IFrameReader frameReader)
        {
            if (frameReader.MessageOpcode != MessageOpcodes.Supported)
            {
                throw new UnknownResponseException(frameReader.MessageOpcode);
            }

            Stream stream = frameReader.ReadOnlyStream;
            Dictionary<string, string[]> res = stream.ReadStringMultimap();
            yield return res;
        }

        protected override Action<IFrameWriter> CreateWriter()
        {
            Action<IFrameWriter> writer = frameWriter => frameWriter.SetMessageType(MessageOpcodes.Options);
            return writer;
        }

        protected override InstrumentationToken CreateToken()
        {
            InstrumentationToken token = InstrumentationToken.Create(RequestType.Options, ExecutionFlags.None);
            return token;
        }
    }
}