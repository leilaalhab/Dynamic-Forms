// source: form.proto
/**
 * @fileoverview
 * @enhanceable
 * @suppress {missingRequire} reports error on implicit type usages.
 * @suppress {messageConventions} JS Compiler reports an error if a variable or
 *     field starts with 'MSG_' and isn't a translatable message.
 * @public
 */
// GENERATED CODE -- DO NOT EDIT!
/* eslint-disable */
// @ts-nocheck

goog.exportSymbol('proto.formpackage.ErrorType', null, global);
goog.exportSymbol('proto.formpackage.InpType', null, global);
goog.exportSymbol('proto.formpackage.Request', null, global);
goog.exportSymbol('proto.formpackage.RequestType', null, global);
goog.exportSymbol('proto.formpackage.Response', null, global);
goog.exportSymbol('proto.formpackage.ResponseType', null, global);
goog.exportSymbol('proto.formpackage.SendChoice', null, global);
/**
 * Generated by JsPbCodeGenerator.
 * @param {Array=} opt_data Optional initial data array, typically from a
 * server response, or constructed directly in Javascript. The array is used
 * in place and becomes part of the constructed object. It is not cloned.
 * If no data is provided, the constructed object will be empty, but still
 * valid. 
 * @extends {jspb.Message}
 * @constructor
 */
proto.formpackage.Request = function(opt_data) {
  jspb.Message.initialize(this, opt_data, 0, -1, null, null);
};
goog.inherits(proto.formpackage.Request, jspb.Message);
if (goog.DEBUG && !COMPILED) {
  /**
   * @public
   * @override
   */
  proto.formpackage.Request.displayName = 'proto.formpackage.Request';
}
/**
 * Generated by JsPbCodeGenerator.
 * @param {Array=} opt_data Optional initial data array, typically from a
 * server response, or constructed directly in Javascript. The array is used
 * in place and becomes part of the constructed object. It is not cloned.
 * If no data is provided, the constructed object will be empty, but still
 * valid.
 * @extends {jspb.Message}
 * @constructor
 */
proto.formpackage.Response = function(opt_data) {
  jspb.Message.initialize(this, opt_data, 0, -1, proto.formpackage.Response.repeatedFields_, null);
};
goog.inherits(proto.formpackage.Response, jspb.Message);
if (goog.DEBUG && !COMPILED) {
  /**
   * @public
   * @override
   */
  proto.formpackage.Response.displayName = 'proto.formpackage.Response';
}
/**
 * Generated by JsPbCodeGenerator.
 * @param {Array=} opt_data Optional initial data array, typically from a
 * server response, or constructed directly in Javascript. The array is used
 * in place and becomes part of the constructed object. It is not cloned.
 * If no data is provided, the constructed object will be empty, but still
 * valid.
 * @extends {jspb.Message}
 * @constructor
 */
proto.formpackage.SendChoice = function(opt_data) {
  jspb.Message.initialize(this, opt_data, 0, -1, null, null);
};
goog.inherits(proto.formpackage.SendChoice, jspb.Message);
if (goog.DEBUG && !COMPILED) {
  /**
   * @public
   * @override
   */
  proto.formpackage.SendChoice.displayName = 'proto.formpackage.SendChoice';
}



if (jspb.Message.GENERATE_TO_OBJECT) {
/**
 * Creates an object representation of this proto.
 * Field names that are reserved in JavaScript and will be renamed to pb_name.
 * Optional fields that are not set will be set to undefined.
 * To access a reserved field use, foo.pb_<name>, eg, foo.pb_default.
 * For the list of reserved names please see:
 *     net/proto2/compiler/js/internal/generator.cc#kKeyword.
 * @param {boolean=} opt_includeInstance Deprecated. whether to include the
 *     JSPB instance for transitional soy proto support:
 *     http://goto/soy-param-migration
 * @return {!Object}
 */
proto.formpackage.Request.prototype.toObject = function(opt_includeInstance) {
  return proto.formpackage.Request.toObject(opt_includeInstance, this);
};


/**
 * Static version of the {@see toObject} method.
 * @param {boolean|undefined} includeInstance Deprecated. Whether to include
 *     the JSPB instance for transitional soy proto support:
 *     http://goto/soy-param-migration
 * @param {!proto.formpackage.Request} msg The msg instance to transform.
 * @return {!Object}
 * @suppress {unusedLocalVariables} f is only used for nested messages
 */
proto.formpackage.Request.toObject = function(includeInstance, msg) {
  var f, obj = {
    id: jspb.Message.getFieldWithDefault(msg, 1, 0),
    inptype: jspb.Message.getFieldWithDefault(msg, 2, 0),
    value: jspb.Message.getFloatingPointFieldWithDefault(msg, 3, 0.0),
    textvalue: jspb.Message.getFieldWithDefault(msg, 4, ""),
    requesttype: jspb.Message.getFieldWithDefault(msg, 5, 0)
  };

  if (includeInstance) {
    obj.$jspbMessageInstance = msg;
  }
  return obj;
};
}


/**
 * Deserializes binary data (in protobuf wire format).
 * @param {jspb.ByteSource} bytes The bytes to deserialize.
 * @return {!proto.formpackage.Request}
 */
proto.formpackage.Request.deserializeBinary = function(bytes) {
  var reader = new jspb.BinaryReader(bytes);
  var msg = new proto.formpackage.Request;
  return proto.formpackage.Request.deserializeBinaryFromReader(msg, reader);
};


/**
 * Deserializes binary data (in protobuf wire format) from the
 * given reader into the given message object.
 * @param {!proto.formpackage.Request} msg The message object to deserialize into.
 * @param {!jspb.BinaryReader} reader The BinaryReader to use.
 * @return {!proto.formpackage.Request}
 */
proto.formpackage.Request.deserializeBinaryFromReader = function(msg, reader) {
  while (reader.nextField()) {
    if (reader.isEndGroup()) {
      break;
    }
    var field = reader.getFieldNumber();
    switch (field) {
    case 1:
      var value = /** @type {number} */ (reader.readInt32());
      msg.setId(value);
      break;
    case 2:
      var value = /** @type {!proto.formpackage.InpType} */ (reader.readEnum());
      msg.setInptype(value);
      break;
    case 3:
      var value = /** @type {number} */ (reader.readFloat());
      msg.setValue(value);
      break;
    case 4:
      var value = /** @type {string} */ (reader.readString());
      msg.setTextvalue(value);
      break;
    case 5:
      var value = /** @type {!proto.formpackage.RequestType} */ (reader.readEnum());
      msg.setRequesttype(value);
      break;
    default:
      reader.skipField();
      break;
    }
  }
  return msg;
};


/**
 * Serializes the message to binary data (in protobuf wire format).
 * @return {!Uint8Array}
 */
proto.formpackage.Request.prototype.serializeBinary = function() {
  var writer = new jspb.BinaryWriter();
  proto.formpackage.Request.serializeBinaryToWriter(this, writer);
  return writer.getResultBuffer();
};


/**
 * Serializes the given message to binary data (in protobuf wire
 * format), writing to the given BinaryWriter.
 * @param {!proto.formpackage.Request} message
 * @param {!jspb.BinaryWriter} writer
 * @suppress {unusedLocalVariables} f is only used for nested messages
 */
proto.formpackage.Request.serializeBinaryToWriter = function(message, writer) {
  var f = undefined;
  f = message.getId();
  if (f !== 0) {
    writer.writeInt32(
      1,
      f
    );
  }
  f = message.getInptype();
  if (f !== 0.0) {
    writer.writeEnum(
      2,
      f
    );
  }
  f = message.getValue();
  if (f !== 0.0) {
    writer.writeFloat(
      3,
      f
    );
  }
  f = message.getTextvalue();
  if (f.length > 0) {
    writer.writeString(
      4,
      f
    );
  }
  f = message.getRequesttype();
  if (f !== 0.0) {
    writer.writeEnum(
      5,
      f
    );
  }
};


/**
 * optional int32 Id = 1;
 * @return {number}
 */
proto.formpackage.Request.prototype.getId = function() {
  return /** @type {number} */ (jspb.Message.getFieldWithDefault(this, 1, 0));
};


/**
 * @param {number} value
 * @return {!proto.formpackage.Request} returns this
 */
proto.formpackage.Request.prototype.setId = function(value) {
  return jspb.Message.setProto3IntField(this, 1, value);
};


/**
 * optional InpType InpType = 2;
 * @return {!proto.formpackage.InpType}
 */
proto.formpackage.Request.prototype.getInptype = function() {
  return /** @type {!proto.formpackage.InpType} */ (jspb.Message.getFieldWithDefault(this, 2, 0));
};


/**
 * @param {!proto.formpackage.InpType} value
 * @return {!proto.formpackage.Request} returns this
 */
proto.formpackage.Request.prototype.setInptype = function(value) {
  return jspb.Message.setProto3EnumField(this, 2, value);
};


/**
 * optional float Value = 3;
 * @return {number}
 */
proto.formpackage.Request.prototype.getValue = function() {
  return /** @type {number} */ (jspb.Message.getFloatingPointFieldWithDefault(this, 3, 0.0));
};


/**
 * @param {number} value
 * @return {!proto.formpackage.Request} returns this
 */
proto.formpackage.Request.prototype.setValue = function(value) {
  return jspb.Message.setProto3FloatField(this, 3, value);
};


/**
 * optional string TextValue = 4;
 * @return {string}
 */
proto.formpackage.Request.prototype.getTextvalue = function() {
  return /** @type {string} */ (jspb.Message.getFieldWithDefault(this, 4, ""));
};


/**
 * @param {string} value
 * @return {!proto.formpackage.Request} returns this
 */
proto.formpackage.Request.prototype.setTextvalue = function(value) {
  return jspb.Message.setProto3StringField(this, 4, value);
};


/**
 * optional RequestType RequestType = 5;
 * @return {!proto.formpackage.RequestType}
 */
proto.formpackage.Request.prototype.getRequesttype = function() {
  return /** @type {!proto.formpackage.RequestType} */ (jspb.Message.getFieldWithDefault(this, 5, 0));
};


/**
 * @param {!proto.formpackage.RequestType} value
 * @return {!proto.formpackage.Request} returns this
 */
proto.formpackage.Request.prototype.setRequesttype = function(value) {
  return jspb.Message.setProto3EnumField(this, 5, value);
};



/**
 * List of repeated fields within this message type.
 * @private {!Array<number>}
 * @const
 */
proto.formpackage.Response.repeatedFields_ = [10];



if (jspb.Message.GENERATE_TO_OBJECT) {
/**
 * Creates an object representation of this proto.
 * Field names that are reserved in JavaScript and will be renamed to pb_name.
 * Optional fields that are not set will be set to undefined.
 * To access a reserved field use, foo.pb_<name>, eg, foo.pb_default.
 * For the list of reserved names please see:
 *     net/proto2/compiler/js/internal/generator.cc#kKeyword.
 * @param {boolean=} opt_includeInstance Deprecated. whether to include the
 *     JSPB instance for transitional soy proto support:
 *     http://goto/soy-param-migration
 * @return {!Object}
 */
proto.formpackage.Response.prototype.toObject = function(opt_includeInstance) {
  return proto.formpackage.Response.toObject(opt_includeInstance, this);
};


/**
 * Static version of the {@see toObject} method.
 * @param {boolean|undefined} includeInstance Deprecated. Whether to include
 *     the JSPB instance for transitional soy proto support:
 *     http://goto/soy-param-migration
 * @param {!proto.formpackage.Response} msg The msg instance to transform.
 * @return {!Object}
 * @suppress {unusedLocalVariables} f is only used for nested messages
 */
proto.formpackage.Response.toObject = function(includeInstance, msg) {
  var f, obj = {
    id: jspb.Message.getFieldWithDefault(msg, 1, 0),
    label: jspb.Message.getFieldWithDefault(msg, 2, ""),
    placeholder: jspb.Message.getFieldWithDefault(msg, 3, ""),
    inptype: jspb.Message.getFieldWithDefault(msg, 4, 0),
    error: jspb.Message.getFieldWithDefault(msg, 5, 0),
    errorvalue: jspb.Message.getFloatingPointFieldWithDefault(msg, 6, 0.0),
    value: jspb.Message.getFloatingPointFieldWithDefault(msg, 7, 0.0),
    textvalue: jspb.Message.getFieldWithDefault(msg, 8, ""),
    responsetype: jspb.Message.getFieldWithDefault(msg, 9, 0),
    choicesList: jspb.Message.toObjectList(msg.getChoicesList(),
    proto.formpackage.SendChoice.toObject, includeInstance)
  };

  if (includeInstance) {
    obj.$jspbMessageInstance = msg;
  }
  return obj;
};
}


/**
 * Deserializes binary data (in protobuf wire format).
 * @param {jspb.ByteSource} bytes The bytes to deserialize.
 * @return {!proto.formpackage.Response}
 */
proto.formpackage.Response.deserializeBinary = function(bytes) {
  var reader = new jspb.BinaryReader(bytes);
  var msg = new proto.formpackage.Response;
  return proto.formpackage.Response.deserializeBinaryFromReader(msg, reader);
};


/**
 * Deserializes binary data (in protobuf wire format) from the
 * given reader into the given message object.
 * @param {!proto.formpackage.Response} msg The message object to deserialize into.
 * @param {!jspb.BinaryReader} reader The BinaryReader to use.
 * @return {!proto.formpackage.Response}
 */
proto.formpackage.Response.deserializeBinaryFromReader = function(msg, reader) {
  while (reader.nextField()) {
    if (reader.isEndGroup()) {
      break;
    }
    var field = reader.getFieldNumber();
    switch (field) {
    case 1:
      var value = /** @type {number} */ (reader.readInt32());
      msg.setId(value);
      break;
    case 2:
      var value = /** @type {string} */ (reader.readString());
      msg.setLabel(value);
      break;
    case 3:
      var value = /** @type {string} */ (reader.readString());
      msg.setPlaceholder(value);
      break;
    case 4:
      var value = /** @type {!proto.formpackage.InpType} */ (reader.readEnum());
      msg.setInptype(value);
      break;
    case 5:
      var value = /** @type {!proto.formpackage.ErrorType} */ (reader.readEnum());
      msg.setError(value);
      break;
    case 6:
      var value = /** @type {number} */ (reader.readDouble());
      msg.setErrorvalue(value);
      break;
    case 7:
      var value = /** @type {number} */ (reader.readDouble());
      msg.setValue(value);
      break;
    case 8:
      var value = /** @type {string} */ (reader.readString());
      msg.setTextvalue(value);
      break;
    case 9:
      var value = /** @type {!proto.formpackage.ResponseType} */ (reader.readEnum());
      msg.setResponsetype(value);
      break;
    case 10:
      var value = new proto.formpackage.SendChoice;
      reader.readMessage(value,proto.formpackage.SendChoice.deserializeBinaryFromReader);
      msg.addChoices(value);
      break;
    default:
      reader.skipField();
      break;
    }
  }
  return msg;
};


/**
 * Serializes the message to binary data (in protobuf wire format).
 * @return {!Uint8Array}
 */
proto.formpackage.Response.prototype.serializeBinary = function() {
  var writer = new jspb.BinaryWriter();
  proto.formpackage.Response.serializeBinaryToWriter(this, writer);
  return writer.getResultBuffer();
};


/**
 * Serializes the given message to binary data (in protobuf wire
 * format), writing to the given BinaryWriter.
 * @param {!proto.formpackage.Response} message
 * @param {!jspb.BinaryWriter} writer
 * @suppress {unusedLocalVariables} f is only used for nested messages
 */
proto.formpackage.Response.serializeBinaryToWriter = function(message, writer) {
  var f = undefined;
  f = message.getId();
  if (f !== 0) {
    writer.writeInt32(
      1,
      f
    );
  }
  f = message.getLabel();
  if (f.length > 0) {
    writer.writeString(
      2,
      f
    );
  }
  f = message.getPlaceholder();
  if (f.length > 0) {
    writer.writeString(
      3,
      f
    );
  }
  f = message.getInptype();
  if (f !== 0.0) {
    writer.writeEnum(
      4,
      f
    );
  }
  f = message.getError();
  if (f !== 0.0) {
    writer.writeEnum(
      5,
      f
    );
  }
  f = message.getErrorvalue();
  if (f !== 0.0) {
    writer.writeDouble(
      6,
      f
    );
  }
  f = message.getValue();
  if (f !== 0.0) {
    writer.writeDouble(
      7,
      f
    );
  }
  f = message.getTextvalue();
  if (f.length > 0) {
    writer.writeString(
      8,
      f
    );
  }
  f = message.getResponsetype();
  if (f !== 0.0) {
    writer.writeEnum(
      9,
      f
    );
  }
  f = message.getChoicesList();
  if (f.length > 0) {
    writer.writeRepeatedMessage(
      10,
      f,
      proto.formpackage.SendChoice.serializeBinaryToWriter
    );
  }
};


/**
 * optional int32 Id = 1;
 * @return {number}
 */
proto.formpackage.Response.prototype.getId = function() {
  return /** @type {number} */ (jspb.Message.getFieldWithDefault(this, 1, 0));
};


/**
 * @param {number} value
 * @return {!proto.formpackage.Response} returns this
 */
proto.formpackage.Response.prototype.setId = function(value) {
  return jspb.Message.setProto3IntField(this, 1, value);
};


/**
 * optional string Label = 2;
 * @return {string}
 */
proto.formpackage.Response.prototype.getLabel = function() {
  return /** @type {string} */ (jspb.Message.getFieldWithDefault(this, 2, ""));
};


/**
 * @param {string} value
 * @return {!proto.formpackage.Response} returns this
 */
proto.formpackage.Response.prototype.setLabel = function(value) {
  return jspb.Message.setProto3StringField(this, 2, value);
};


/**
 * optional string Placeholder = 3;
 * @return {string}
 */
proto.formpackage.Response.prototype.getPlaceholder = function() {
  return /** @type {string} */ (jspb.Message.getFieldWithDefault(this, 3, ""));
};


/**
 * @param {string} value
 * @return {!proto.formpackage.Response} returns this
 */
proto.formpackage.Response.prototype.setPlaceholder = function(value) {
  return jspb.Message.setProto3StringField(this, 3, value);
};


/**
 * optional InpType InpType = 4;
 * @return {!proto.formpackage.InpType}
 */
proto.formpackage.Response.prototype.getInptype = function() {
  return /** @type {!proto.formpackage.InpType} */ (jspb.Message.getFieldWithDefault(this, 4, 0));
};


/**
 * @param {!proto.formpackage.InpType} value
 * @return {!proto.formpackage.Response} returns this
 */
proto.formpackage.Response.prototype.setInptype = function(value) {
  return jspb.Message.setProto3EnumField(this, 4, value);
};


/**
 * optional ErrorType Error = 5;
 * @return {!proto.formpackage.ErrorType}
 */
proto.formpackage.Response.prototype.getError = function() {
  return /** @type {!proto.formpackage.ErrorType} */ (jspb.Message.getFieldWithDefault(this, 5, 0));
};


/**
 * @param {!proto.formpackage.ErrorType} value
 * @return {!proto.formpackage.Response} returns this
 */
proto.formpackage.Response.prototype.setError = function(value) {
  return jspb.Message.setProto3EnumField(this, 5, value);
};


/**
 * optional double ErrorValue = 6;
 * @return {number}
 */
proto.formpackage.Response.prototype.getErrorvalue = function() {
  return /** @type {number} */ (jspb.Message.getFloatingPointFieldWithDefault(this, 6, 0.0));
};


/**
 * @param {number} value
 * @return {!proto.formpackage.Response} returns this
 */
proto.formpackage.Response.prototype.setErrorvalue = function(value) {
  return jspb.Message.setProto3FloatField(this, 6, value);
};


/**
 * optional double Value = 7;
 * @return {number}
 */
proto.formpackage.Response.prototype.getValue = function() {
  return /** @type {number} */ (jspb.Message.getFloatingPointFieldWithDefault(this, 7, 0.0));
};


/**
 * @param {number} value
 * @return {!proto.formpackage.Response} returns this
 */
proto.formpackage.Response.prototype.setValue = function(value) {
  return jspb.Message.setProto3FloatField(this, 7, value);
};


/**
 * optional string TextValue = 8;
 * @return {string}
 */
proto.formpackage.Response.prototype.getTextvalue = function() {
  return /** @type {string} */ (jspb.Message.getFieldWithDefault(this, 8, ""));
};


/**
 * @param {string} value
 * @return {!proto.formpackage.Response} returns this
 */
proto.formpackage.Response.prototype.setTextvalue = function(value) {
  return jspb.Message.setProto3StringField(this, 8, value);
};


/**
 * optional ResponseType ResponseType = 9;
 * @return {!proto.formpackage.ResponseType}
 */
proto.formpackage.Response.prototype.getResponsetype = function() {
  return /** @type {!proto.formpackage.ResponseType} */ (jspb.Message.getFieldWithDefault(this, 9, 0));
};


/**
 * @param {!proto.formpackage.ResponseType} value
 * @return {!proto.formpackage.Response} returns this
 */
proto.formpackage.Response.prototype.setResponsetype = function(value) {
  return jspb.Message.setProto3EnumField(this, 9, value);
};


/**
 * repeated SendChoice Choices = 10;
 * @return {!Array<!proto.formpackage.SendChoice>}
 */
proto.formpackage.Response.prototype.getChoicesList = function() {
  return /** @type{!Array<!proto.formpackage.SendChoice>} */ (
    jspb.Message.getRepeatedWrapperField(this, proto.formpackage.SendChoice, 10));
};


/**
 * @param {!Array<!proto.formpackage.SendChoice>} value
 * @return {!proto.formpackage.Response} returns this
*/
proto.formpackage.Response.prototype.setChoicesList = function(value) {
  return jspb.Message.setRepeatedWrapperField(this, 10, value);
};


/**
 * @param {!proto.formpackage.SendChoice=} opt_value
 * @param {number=} opt_index
 * @return {!proto.formpackage.SendChoice}
 */
proto.formpackage.Response.prototype.addChoices = function(opt_value, opt_index) {
  return jspb.Message.addToRepeatedWrapperField(this, 10, opt_value, proto.formpackage.SendChoice, opt_index);
};


/**
 * Clears the list making it empty but non-null.
 * @return {!proto.formpackage.Response} returns this
 */
proto.formpackage.Response.prototype.clearChoicesList = function() {
  return this.setChoicesList([]);
};





if (jspb.Message.GENERATE_TO_OBJECT) {
/**
 * Creates an object representation of this proto.
 * Field names that are reserved in JavaScript and will be renamed to pb_name.
 * Optional fields that are not set will be set to undefined.
 * To access a reserved field use, foo.pb_<name>, eg, foo.pb_default.
 * For the list of reserved names please see:
 *     net/proto2/compiler/js/internal/generator.cc#kKeyword.
 * @param {boolean=} opt_includeInstance Deprecated. whether to include the
 *     JSPB instance for transitional soy proto support:
 *     http://goto/soy-param-migration
 * @return {!Object}
 */
proto.formpackage.SendChoice.prototype.toObject = function(opt_includeInstance) {
  return proto.formpackage.SendChoice.toObject(opt_includeInstance, this);
};


/**
 * Static version of the {@see toObject} method.
 * @param {boolean|undefined} includeInstance Deprecated. Whether to include
 *     the JSPB instance for transitional soy proto support:
 *     http://goto/soy-param-migration
 * @param {!proto.formpackage.SendChoice} msg The msg instance to transform.
 * @return {!Object}
 * @suppress {unusedLocalVariables} f is only used for nested messages
 */
proto.formpackage.SendChoice.toObject = function(includeInstance, msg) {
  var f, obj = {
    id: jspb.Message.getFieldWithDefault(msg, 1, 0),
    label: jspb.Message.getFieldWithDefault(msg, 2, "")
  };

  if (includeInstance) {
    obj.$jspbMessageInstance = msg;
  }
  return obj;
};
}


/**
 * Deserializes binary data (in protobuf wire format).
 * @param {jspb.ByteSource} bytes The bytes to deserialize.
 * @return {!proto.formpackage.SendChoice}
 */
proto.formpackage.SendChoice.deserializeBinary = function(bytes) {
  var reader = new jspb.BinaryReader(bytes);
  var msg = new proto.formpackage.SendChoice;
  return proto.formpackage.SendChoice.deserializeBinaryFromReader(msg, reader);
};


/**
 * Deserializes binary data (in protobuf wire format) from the
 * given reader into the given message object.
 * @param {!proto.formpackage.SendChoice} msg The message object to deserialize into.
 * @param {!jspb.BinaryReader} reader The BinaryReader to use.
 * @return {!proto.formpackage.SendChoice}
 */
proto.formpackage.SendChoice.deserializeBinaryFromReader = function(msg, reader) {
  while (reader.nextField()) {
    if (reader.isEndGroup()) {
      break;
    }
    var field = reader.getFieldNumber();
    switch (field) {
    case 1:
      var value = /** @type {number} */ (reader.readInt32());
      msg.setId(value);
      break;
    case 2:
      var value = /** @type {string} */ (reader.readString());
      msg.setLabel(value);
      break;
    default:
      reader.skipField();
      break;
    }
  }
  return msg;
};


/**
 * Serializes the message to binary data (in protobuf wire format).
 * @return {!Uint8Array}
 */
proto.formpackage.SendChoice.prototype.serializeBinary = function() {
  var writer = new jspb.BinaryWriter();
  proto.formpackage.SendChoice.serializeBinaryToWriter(this, writer);
  return writer.getResultBuffer();
};


/**
 * Serializes the given message to binary data (in protobuf wire
 * format), writing to the given BinaryWriter.
 * @param {!proto.formpackage.SendChoice} message
 * @param {!jspb.BinaryWriter} writer
 * @suppress {unusedLocalVariables} f is only used for nested messages
 */
proto.formpackage.SendChoice.serializeBinaryToWriter = function(message, writer) {
  var f = undefined;
  f = message.getId();
  if (f !== 0) {
    writer.writeInt32(
      1,
      f
    );
  }
  f = message.getLabel();
  if (f.length > 0) {
    writer.writeString(
      2,
      f
    );
  }
};


/**
 * optional int32 Id = 1;
 * @return {number}
 */
proto.formpackage.SendChoice.prototype.getId = function() {
  return /** @type {number} */ (jspb.Message.getFieldWithDefault(this, 1, 0));
};


/**
 * @param {number} value
 * @return {!proto.formpackage.SendChoice} returns this
 */
proto.formpackage.SendChoice.prototype.setId = function(value) {
  return jspb.Message.setProto3IntField(this, 1, value);
};


/**
 * optional string Label = 2;
 * @return {string}
 */
proto.formpackage.SendChoice.prototype.getLabel = function() {
  return /** @type {string} */ (jspb.Message.getFieldWithDefault(this, 2, ""));
};


/**
 * @param {string} value
 * @return {!proto.formpackage.SendChoice} returns this
 */
proto.formpackage.SendChoice.prototype.setLabel = function(value) {
  return jspb.Message.setProto3StringField(this, 2, value);
};


/**
 * @enum {number}
 */
proto.formpackage.InpType = {
  TEXT: 0,
  INTEGER: 1,
  FLOAT: 2,
  DATETIME: 3,
  RADIO: 4,
  CHECKBOX: 5,
  OPTIONS: 6
};

/**
 * @enum {number}
 */
proto.formpackage.RequestType = {
  INPUTVALUE: 0,
  FORMSUBMIT: 1
};

/**
 * @enum {number}
 */
proto.formpackage.ResponseType = {
  INPUTVALIDITY: 0,
  NEWINPUT: 1,
  FORMSUBMITACCEPTED: 2,
  FORMSUBMITREJECTED: 3,
  PRICE: 4
};

/**
 * @enum {number}
 */
proto.formpackage.ErrorType = {
  REQUIRED: 0,
  EQUAL: 1,
  NOTEQUAL: 2,
  GREATERTHAN: 3,
  LESSTHAN: 4,
  NOERROR: 5
};

