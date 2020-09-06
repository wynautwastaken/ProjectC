function gridsnap(num) {
	return (num div 8) * 8;
}
function UpdateTilesNear(x, y) {
	if(position_meeting(x-8,y,obj_tile_1x1)) {
		with(instance_position(x-8,y,obj_tile_1x1)) {
			ind = UpdateTile();	
		}
	}
	if(position_meeting(x+8,y,obj_tile_1x1)) {
		with(instance_position(x+8,y,obj_tile_1x1)) {
			ind = UpdateTile();	
		}
	}
	if(position_meeting(x,y+8,obj_tile_1x1)) {
		with(instance_position(x,y+8,obj_tile_1x1)) {
			ind = UpdateTile();	
		}
	}
	if(position_meeting(x,y-8,obj_tile_1x1)) {
		with(instance_position(x,y-8,obj_tile_1x1)) {
			ind = UpdateTile();	
		}
	}
}