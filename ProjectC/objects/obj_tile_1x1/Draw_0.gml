draw_sprite_part(sprite_index,image_index,(ind mod 4) * 8,(ind div 4) * 8,8,8,x,y);
if(transition) {
	draw_sprite_part(other_sprite,image_index,(ind mod 4) * 8,(ind div 4) * 8,8,8,x,y);
	draw_sprite_part(GetTransition(other_sprite),image_index,(t_ind mod 4) * 8,(t_ind div 4) * 8,8,8,x,y);
	if(sprite_index == spr_tile_stone) {
		draw_sprite_part(spr_tile_stone_overlay,image_index,(ind mod 4) * 8,(ind div 4) * 8,8,8,x,y);
	}
}