// Copyright (c) 2008-2020, Hazelcast, Inc. All Rights Reserved.
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

// <auto-generated>
//   This code was generated by a tool.
//     Hazelcast Client Protocol Code Generator
//     https://github.com/hazelcast/hazelcast-client-protocol
//   Change to this file will be lost if the code is regenerated.
// </auto-generated>

#pragma warning disable IDE0051 // Remove unused private members
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hazelcast.Protocol.BuiltInCodecs;
using Hazelcast.Protocol.CustomCodecs;
using Hazelcast.Core;
using Hazelcast.Messaging;
using Hazelcast.Clustering;
using Hazelcast.Serialization;
using Microsoft.Extensions.Logging;

namespace Hazelcast.Protocol.Codecs
{
    /// <summary>
    /// Makes an authentication request to the cluster using custom credentials.
    ///</summary>
#if SERVER_CODEC
    internal static class ClientAuthenticationCustomServerCodec
#else
    internal static class ClientAuthenticationCustomCodec
#endif
    {
        public const int RequestMessageType = 512; // 0x000200
        public const int ResponseMessageType = 513; // 0x000201
        private const int RequestUuidFieldOffset = Messaging.FrameFields.Offset.PartitionId + BytesExtensions.SizeOfInt;
        private const int RequestSerializationVersionFieldOffset = RequestUuidFieldOffset + BytesExtensions.SizeOfGuid;
        private const int RequestInitialFrameSize = RequestSerializationVersionFieldOffset + BytesExtensions.SizeOfByte;
        private const int ResponseStatusFieldOffset = Messaging.FrameFields.Offset.ResponseBackupAcks + BytesExtensions.SizeOfByte;
        private const int ResponseMemberUuidFieldOffset = ResponseStatusFieldOffset + BytesExtensions.SizeOfByte;
        private const int ResponseSerializationVersionFieldOffset = ResponseMemberUuidFieldOffset + BytesExtensions.SizeOfGuid;
        private const int ResponsePartitionCountFieldOffset = ResponseSerializationVersionFieldOffset + BytesExtensions.SizeOfByte;
        private const int ResponseClusterIdFieldOffset = ResponsePartitionCountFieldOffset + BytesExtensions.SizeOfInt;
        private const int ResponseFailoverSupportedFieldOffset = ResponseClusterIdFieldOffset + BytesExtensions.SizeOfGuid;
        private const int ResponseInitialFrameSize = ResponseFailoverSupportedFieldOffset + BytesExtensions.SizeOfBool;

#if SERVER_CODEC
        public sealed class RequestParameters
        {

            /// <summary>
            /// Cluster name that client will connect to.
            ///</summary>
            public string ClusterName { get; set; }

            /// <summary>
            /// Secret byte array for authentication.
            ///</summary>
            public byte[] Credentials { get; set; }

            /// <summary>
            /// Unique string identifying the connected client uniquely.
            ///</summary>
            public Guid Uuid { get; set; }

            /// <summary>
            /// The type of the client. E.g. JAVA, CPP, CSHARP, etc.
            ///</summary>
            public string ClientType { get; set; }

            /// <summary>
            /// client side supported version to inform server side
            ///</summary>
            public byte SerializationVersion { get; set; }

            /// <summary>
            /// The Hazelcast version of the client. (e.g. 3.7.2)
            ///</summary>
            public string ClientHazelcastVersion { get; set; }

            /// <summary>
            /// the name of the client instance
            ///</summary>
            public string ClientName { get; set; }

            /// <summary>
            /// User defined labels of the client instance
            ///</summary>
            public IList<string> Labels { get; set; }
        }
#endif

        public static ClientMessage EncodeRequest(string clusterName, byte[] credentials, Guid uuid, string clientType, byte serializationVersion, string clientHazelcastVersion, string clientName, ICollection<string> labels)
        {
            var clientMessage = new ClientMessage
            {
                IsRetryable = true,
                OperationName = "Client.AuthenticationCustom"
            };
            var initialFrame = new Frame(new byte[RequestInitialFrameSize], (FrameFlags) ClientMessageFlags.Unfragmented);
            initialFrame.Bytes.WriteIntL(Messaging.FrameFields.Offset.MessageType, RequestMessageType);
            initialFrame.Bytes.WriteIntL(Messaging.FrameFields.Offset.PartitionId, -1);
            initialFrame.Bytes.WriteGuidL(RequestUuidFieldOffset, uuid);
            initialFrame.Bytes.WriteByteL(RequestSerializationVersionFieldOffset, serializationVersion);
            clientMessage.Append(initialFrame);
            StringCodec.Encode(clientMessage, clusterName);
            ByteArrayCodec.Encode(clientMessage, credentials);
            StringCodec.Encode(clientMessage, clientType);
            StringCodec.Encode(clientMessage, clientHazelcastVersion);
            StringCodec.Encode(clientMessage, clientName);
            ListMultiFrameCodec.Encode(clientMessage, labels, StringCodec.Encode);
            return clientMessage;
        }

#if SERVER_CODEC
        public static RequestParameters DecodeRequest(ClientMessage clientMessage)
        {
            using var iterator = clientMessage.GetEnumerator();
            var request = new RequestParameters();
            var initialFrame = iterator.Take();
            request.Uuid = initialFrame.Bytes.ReadGuidL(RequestUuidFieldOffset);
            request.SerializationVersion = initialFrame.Bytes.ReadByteL(RequestSerializationVersionFieldOffset);
            request.ClusterName = StringCodec.Decode(iterator);
            request.Credentials = ByteArrayCodec.Decode(iterator);
            request.ClientType = StringCodec.Decode(iterator);
            request.ClientHazelcastVersion = StringCodec.Decode(iterator);
            request.ClientName = StringCodec.Decode(iterator);
            request.Labels = ListMultiFrameCodec.Decode(iterator, StringCodec.Decode);
            return request;
        }
#endif

        public sealed class ResponseParameters
        {

            /// <summary>
            /// A byte that represents the authentication status. It can be AUTHENTICATED(0), CREDENTIALS_FAILED(1),
            /// SERIALIZATION_VERSION_MISMATCH(2) or NOT_ALLOWED_IN_CLUSTER(3).
            ///</summary>
            public byte Status { get; set; }

            /// <summary>
            /// Address of the Hazelcast member which sends the authentication response.
            ///</summary>
            public Hazelcast.Networking.NetworkAddress Address { get; set; }

            /// <summary>
            /// UUID of the Hazelcast member which sends the authentication response.
            ///</summary>
            public Guid MemberUuid { get; set; }

            /// <summary>
            /// client side supported version to inform server side
            ///</summary>
            public byte SerializationVersion { get; set; }

            /// <summary>
            /// Version of the Hazelcast member which sends the authentication response.
            ///</summary>
            public string ServerHazelcastVersion { get; set; }

            /// <summary>
            /// Partition count of the cluster.
            ///</summary>
            public int PartitionCount { get; set; }

            /// <summary>
            /// The cluster id of the cluster.
            ///</summary>
            public Guid ClusterId { get; set; }

            /// <summary>
            /// Returns true if server supports clients with failover feature.
            ///</summary>
            public bool FailoverSupported { get; set; }
        }

#if SERVER_CODEC
        public static ClientMessage EncodeResponse(byte status, Hazelcast.Networking.NetworkAddress address, Guid memberUuid, byte serializationVersion, string serverHazelcastVersion, int partitionCount, Guid clusterId, bool failoverSupported)
        {
            var clientMessage = new ClientMessage();
            var initialFrame = new Frame(new byte[ResponseInitialFrameSize], (FrameFlags) ClientMessageFlags.Unfragmented);
            initialFrame.Bytes.WriteIntL(Messaging.FrameFields.Offset.MessageType, ResponseMessageType);
            initialFrame.Bytes.WriteByteL(ResponseStatusFieldOffset, status);
            initialFrame.Bytes.WriteGuidL(ResponseMemberUuidFieldOffset, memberUuid);
            initialFrame.Bytes.WriteByteL(ResponseSerializationVersionFieldOffset, serializationVersion);
            initialFrame.Bytes.WriteIntL(ResponsePartitionCountFieldOffset, partitionCount);
            initialFrame.Bytes.WriteGuidL(ResponseClusterIdFieldOffset, clusterId);
            initialFrame.Bytes.WriteBoolL(ResponseFailoverSupportedFieldOffset, failoverSupported);
            clientMessage.Append(initialFrame);
            CodecUtil.EncodeNullable(clientMessage, address, AddressCodec.Encode);
            StringCodec.Encode(clientMessage, serverHazelcastVersion);
            return clientMessage;
        }
#endif

        public static ResponseParameters DecodeResponse(ClientMessage clientMessage)
        {
            using var iterator = clientMessage.GetEnumerator();
            var response = new ResponseParameters();
            var initialFrame = iterator.Take();
            response.Status = initialFrame.Bytes.ReadByteL(ResponseStatusFieldOffset);
            response.MemberUuid = initialFrame.Bytes.ReadGuidL(ResponseMemberUuidFieldOffset);
            response.SerializationVersion = initialFrame.Bytes.ReadByteL(ResponseSerializationVersionFieldOffset);
            response.PartitionCount = initialFrame.Bytes.ReadIntL(ResponsePartitionCountFieldOffset);
            response.ClusterId = initialFrame.Bytes.ReadGuidL(ResponseClusterIdFieldOffset);
            response.FailoverSupported = initialFrame.Bytes.ReadBoolL(ResponseFailoverSupportedFieldOffset);
            response.Address = CodecUtil.DecodeNullable(iterator, AddressCodec.Decode);
            response.ServerHazelcastVersion = StringCodec.Decode(iterator);
            return response;
        }

    }
}
