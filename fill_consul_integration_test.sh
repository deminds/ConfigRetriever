export CONSUL_HTTP_ADDR=consul:8500

curl --request PUT --data "10" http://$CONSUL_HTTP_ADDR/v1/kv/FakePath0/FakePath1/TestClass1/PropTestClass1_1_FakeName/PropInt
curl --request PUT --data "10.10" http://$CONSUL_HTTP_ADDR/v1/kv/FakePath0/FakePath1/TestClass1/PropTestClass1_1_FakeName/PropDoubleFakeName
curl --request PUT --data "some string" http://$CONSUL_HTTP_ADDR/v1/kv/FakePath0/FakePath1/PropTestClass2/PropString
curl --request PUT --data "true" http://$CONSUL_HTTP_ADDR/v1/kv/FakePath0/PropTestClass3/PropBool
curl --request PUT --data "some string" http://$CONSUL_HTTP_ADDR/v1/kv/FakePath0/PropTestClass3/PropString
curl --request PUT --data "1000" http://$CONSUL_HTTP_ADDR/v1/kv/PropLongFakeName
