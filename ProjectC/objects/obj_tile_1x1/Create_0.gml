enum SIDES {
	ALL,
	TOPLEFT,
	TOPMID,
	TOPRIGHT,
	TOP,
	MIDLEFT,
	NONE,
	MIDRIGHT,
	MIDV,
	BOTLEFT,
	BOTMID,
	BOTRIGHT,
	BOT,
	LEFT,
	MIDH,
	RIGHT
}
if(place_meeting(x,y,obj_tile_1x1)) {
	instance_destroy(instance_place(x,y,obj_tile_1x1));	
}
ind = SIDES.NONE;

UpdateTile = function() {
	var n_ind = SIDES.ALL, left = false, right = false, up = false, down = false;
	if(place_meeting(x-8,y,obj_tile_1x1)) {
		left = true;
	}
	if(place_meeting(x+8,y,obj_tile_1x1)) {
		right = true;
	}
	if(place_meeting(x,y-8,obj_tile_1x1)) {
		up = true;
	}
	if(place_meeting(x,y+8,obj_tile_1x1)) {
		down = true;
	}
	
	if(left && !right && !up && !down) {
		n_ind = SIDES.RIGHT;
	}
	if(left && right && !up && !down) {
		n_ind = SIDES.MIDH;	
	}
	if(left && up && !right && !down) {
		n_ind = SIDES.BOTRIGHT;	
	}
	if(left && down && !right && !up) {
		n_ind = SIDES.TOPRIGHT;	
	}	
	
	if(right && !left && !up && !down) {
		n_ind = SIDES.LEFT;
	}
	if(right && up && !left && !down) {
		n_ind = SIDES.BOTLEFT;	
	}
	if(right && down && !left && !up) {
		n_ind = SIDES.TOPLEFT;	
	}
	
	if(up && !down && !left && !right) {
		n_ind = SIDES.BOT;	
	}
	if(up && down && !left && !right) {
		n_ind = SIDES.MIDV;
	}
	
	if(down && !up && !left && !right) {
		n_ind = SIDES.TOP;
	}
	
	if(left && right && down && !up) {
		n_ind = SIDES.TOPMID;	
	}
	if(left && right && up && !down) {
		n_ind = SIDES.BOTMID;	
	}
	
	if(up && down && left && !right) {
		n_ind = SIDES.MIDRIGHT;	
	}
	if(up && down && right && !left) {
		n_ind = SIDES.MIDLEFT;	
	}
	
	if(right && left && up && down) {
		n_ind = SIDES.NONE;	
	}
	
	return n_ind;
}
alarm_set(0,2);
