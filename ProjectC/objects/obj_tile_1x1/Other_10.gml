if(place_meeting(x,y,obj_tile_1x1)) {
	instance_destroy(instance_place(x,y,obj_tile_1x1));	
}

ind = UpdateTile();

t_ind = UpdateTileTrans();

UpdateTilesNear(x,y);
if(sprite_index == spr_tile_stone) {
	transition = true;	
}