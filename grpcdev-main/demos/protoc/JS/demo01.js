// protoc --js_out=import_style=commonjs,binary:./JS ./first.proto
//npm install google-protobuf --save

var messages = require("./first_pb");

var p1 = new messages.Person();
console.log(p1.toObject());
var binary = p1.serializeBinary();
console.log(binary);
