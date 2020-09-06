if(mouse_check_button_pressed(mb_left)) {
	//instance_create_layer(gridsnap(mouse_x),gridsnap(mouse_y),layer,obj_tile_1x1);
	
	var position = CalcChunk(mouse_x div 8,mouse_y div 8);
	var buffer = packet_set_block(position.chunk_x,position.chunk_y,position.x,position.y,types.grass_dirt);
	network_send_packet(obj_client.socket,buffer,buffer_tell(buffer));
	buffer_delete(buffer);
}
if(mouse_check_button_pressed(mb_right)) {
	if (position_meeting(gridsnap(mouse_x),gridsnap(mouse_y),obj_tile_1x1)) {
		var inst = instance_position(gridsnap(mouse_x),gridsnap(mouse_y),obj_tile_1x1);
		
		var position = CalcChunk(mouse_x div 8,mouse_y div 8);
		
		var buffer = packet_set_block(position.chunk_x,position.chunk_y,position.x,position.y,types.air);
		network_send_packet(obj_client.socket,buffer,buffer_tell(buffer));
		buffer_delete(buffer);
		
		//instance_destroy(inst);
		//UpdateTilesNear(gridsnap(mouse_x),gridsnap(mouse_y));
	}
}