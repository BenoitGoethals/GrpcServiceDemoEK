//https://github.com/protocolbuffers/protobuf/tree/master/js
//protoc --js_out=import_style=commonjs,binary:./JS ./second.proto
//npm install google-protobuf --save

var messages = require("./second_pb");

var p1 = new messages.Person();
p1.setId(10);
p1.setName("Simona");
console.log(p1.toObject());
var binary = p1.serializeBinary();
console.log(binary);
console.log(new Buffer(binary).toString('base64'));

var p2 = messages.Person.deserializeBinary(binary);
console.log(p2.toObject());
