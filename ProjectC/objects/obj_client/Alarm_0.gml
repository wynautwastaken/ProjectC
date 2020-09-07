var buffer = packet_ping(CLIENT_SIDE);
network_send_packet(socket,buffer,buffer_tell(buffer));
buffer_delete(buffer);
alarm_set(0,30);