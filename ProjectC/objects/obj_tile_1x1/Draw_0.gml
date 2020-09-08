if(transition) {
	draw_sprite_part(other_sprite,image_index,(ind mod 4) * 8,(ind div 4) * 8,8,8,x,y);
	draw_sprite_part(GetTransition(sprite_index),image_index,(t_ind mod 4) * 8,(t_ind div 4) * 8,8,8,x,y);
	draw_sprite_part(GetOverlay(sprite_index),image_index,(ind mod 4) * 8,(ind div 4) * 8,8,8,x,y);
}
else {
	draw_sprite_part(sprite_index,image_index,(ind mod 4) * 8,(ind div 4) * 8,8,8,x,y);	
}