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
transition = false;
GetTransition = function(sprite) {
	return spr_tile_stone_transition_natural;	
}


UpdateTileTrans = function() {
	var n_ind = SIDES.NONE, left = false, right = false, up = false, down = false;
	if(place_meeting(x-8,y,obj_tile_1x1)) {
		var inst = instance_place(x-8,y,obj_tile_1x1);
		if(inst.sprite_index != sprite_index) {
			other_sprite = inst.sprite_index;
			left = true;
		}
	}
	if(place_meeting(x+8,y,obj_tile_1x1)) {
		var inst = instance_place(x+8,y,obj_tile_1x1);
		if(inst.sprite_index != sprite_index) {
			other_sprite = inst.sprite_index;
			right = true;
		}
	}
	if(place_meeting(x,y-8,obj_tile_1x1)) {
		var inst = instance_place(x,y-8,obj_tile_1x1);
		if(inst.sprite_index != sprite_index) {
			other_sprite = inst.sprite_index;
			up = true;
		}
	}
	if(place_meeting(x,y+8,obj_tile_1x1)) {
		var inst = instance_place(x,y+8,obj_tile_1x1);
		if(inst.sprite_index != sprite_index) {
			other_sprite = inst.sprite_index;
			down = true;
		}
	}
	
	if(left && !right && !up && !down) {
		n_ind = SIDES.MIDLEFT; //
	}
	if(left && right && !up && !down) {
		n_ind = SIDES.MIDV;	
	}
	if(left && up && !right && !down) {
		n_ind = SIDES.TOPLEFT;	 //
	}
	if(left && down && !right && !up) {
		n_ind = SIDES.BOTLEFT; //	
	}	
	
	if(right && !left && !up && !down) {
		n_ind = SIDES.MIDRIGHT; //
	}
	if(right && up && !left && !down) {
		n_ind = SIDES.TOPRIGHT; //	
	}
	if(right && down && !left && !up) {
		n_ind = SIDES.BOTRIGHT;	//
	}
	
	if(up && !down && !left && !right) {
		n_ind = SIDES.TOPMID; //	
	}
	if(up && down && !left && !right) {
		n_ind = SIDES.MIDH;
	}
	
	if(down && !up && !left && !right) {
		n_ind = SIDES.BOTMID;
	}
	
	if(left && right && down && !up) {
		n_ind = SIDES.BOT;	
	}
	if(left && right && up && !down) {
		n_ind = SIDES.TOP;
	}
	
	if(up && down && left && !right) {
		n_ind = SIDES.LEFT;
	}
	if(up && down && right && !left) {
		n_ind = SIDES.RIGHT;
	}
	
	if(right && left && up && down) {
		n_ind = SIDES.ALL;	
	}
	
	return n_ind;
}


UpdateTile = function() {
	switch(tile) {
		case types.dirt:
			sprite_index = spr_tile_dirt;
			break;
		case types.grass_dirt:
			sprite_index = spr_tile_dirt_grassy;
			break;
		case types.stone:
			sprite_index = spr_tile_stone;
			break;
	}
	
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

ind = SIDES.NONE;
t_ind = SIDES.ALL;

other_sprite = spr_tile_dirt;

tile = types.grass_dirt;

alarm_set(0,2);