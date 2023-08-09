using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DynamicForms.Models
{
  public class Node
  {
    [BsonIgnoreIfNull]
    public double? InputId { get; set; }

    [BsonIgnoreIfNull]
    public double? Value { get; set; }

    [BsonIgnoreIfNull]
    public Node? Left { get; set; }

    [BsonIgnoreIfNull]
    public Node? Right { get; set; }
    
    public NodeType Type { get; set; }

    public Node(double Value, double InputId, NodeType Type)
    {
      this.Type = Type;
      this.InputId = InputId;
      
    }
    public Node(double Value, NodeType Type)
    {
      this.Type = Type;
      this.Value = Value;
      
    }

    public Node(NodeType Type) {
      this.Type = Type;
    }
    public Node() { }

  }
}