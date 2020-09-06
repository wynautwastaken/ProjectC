if (keyboard_check_pressed(ord("S"))) {
	instance_create_layer(x,y,layer,obj_server);
	instance_create_layer(x,y,layer,obj_client);
	room_goto_next();
	instance_destroy();
}
if (keyboard_check_pressed(ord("C"))) {
	instance_create_layer(x,y,layer,obj_client);
	room_goto_next();
	instance_destroy();
}