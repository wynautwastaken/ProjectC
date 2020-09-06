if(mouse_check_button_pressed(mb_left)) {
	instance_create_layer(gridsnap(mouse_x),gridsnap(mouse_y),layer,obj_tile_1x1);
}
if(mouse_check_button_pressed(mb_right)) {
	show_debug_message("trying to erase...");
	if(position_meeting(gridsnap(mouse_x),gridsnap(mouse_y),obj_tile_1x1)) {
		show_debug_message("erasing tile at" + string(mouse_x div 8) + "," + string(mouse_y div 8));
		var inst = instance_position(gridsnap(mouse_x),gridsnap(mouse_y),obj_tile_1x1);
		instance_destroy(inst);
		UpdateTilesNear(gridsnap(mouse_x),gridsnap(mouse_y));
	}
}