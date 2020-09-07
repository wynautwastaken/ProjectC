if(mouse_check_button_pressed(mb_left)) {
	//instance_create_layer(gridsnap(mouse_x),gridsnap(mouse_y),layer,obj_tile_1x1);
	
	var position = CalcChunk(mouse_x div 8,mouse_y div 8);
	var buffer = packet_set_block(CLIENT_SIDE,position.chunk_x,position.chunk_y,position.x,position.y,tile);
	network_send_packet(obj_client.socket,buffer,buffer_tell(buffer));
	with(obj_client) client_handle_setblock(buffer);
	buffer_delete(buffer);
}

if(mouse_check_button_pressed(mb_right)) {
	var position = CalcChunk(mouse_x div 8,mouse_y div 8);
		
	var buffer = packet_set_block(CLIENT_SIDE,position.chunk_x,position.chunk_y,position.x,position.y,types.air);
	network_send_packet(obj_client.socket,buffer,buffer_tell(buffer));
	with(obj_client) client_handle_setblock(buffer);
	buffer_delete(buffer);
}

if(mouse_wheel_up()) {
	tile++;
	if(tile > types.stone) {
		tile = types.dirt;	
	}
}
if(mouse_wheel_down()) {
	tile--;
	if(tile == types.air) {
		tile = types.stone;
	}
}