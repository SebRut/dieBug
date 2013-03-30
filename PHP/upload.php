<?php
if(!isset($_FILES['datei']['name']) || !isset($_POST['description'])) {
	for($i=0; $i<=1000; $i++) {
		echo "error<br/>";
	}
	die();
}
$dateityp = GetImageSize($_FILES['datei']['tmp_name']);
//if($dateityp[2] == 3) {
	if($_FILES['datei']['size'] <  5242880) {
		$destdir = md5($_SERVER['REMOTE_ADDR']+time());
		
		$newdir = true;
		
		while($newdir == true) {
			if(file_exists("./".$destdir) == true) {
				$destdir = $destdir.'1';
			} else {
				$newdir = false;
			}
		}
		mkdir("./".$destdir, 0777);
		
		$ssxml = new SimpleXMLElement("<screenshot></screenshot>");
		$ssxml->addAttribute('filename', $_FILES['datei']['name']);
		$ssxml->addAttribute('ip', $_SERVER['REMOTE_ADDR']);
		$ssxml->addAttribute('timestamp', time());
		$description = $ssxml->addChild('content',$_POST['description']);
		$ssxml->asXML("./".$destdir."/screenshot.xml");
		
		move_uploaded_file($_FILES['datei']['tmp_name'], "./".$destdir."/".$_FILES['datei']['name']);
		
		$img = imagecreatefrompng("./".$destdir."/".$_FILES['datei']['name']);
		$width = imagesx( $img );
		$height = imagesy( $img );
		$new_width = 260;
		$new_height = floor( $height * ( 260 / $width ) );
  
		// create a new temporary image
		$tmp_img = imagecreatetruecolor( $new_width, $new_height );
  
		// copy and resize old image into new image 
		imagecopyresized( $tmp_img, $img, 0, 0, 0, 0, $new_width, $new_height, $width, $height );
  
		// save thumbnail into a file
		imagepng( $tmp_img, "./".$destdir."/".$_FILES['datei']['name'].".thumb.png" );
		
		echo "http://".$_SERVER['SERVER_NAME'].dirname($_SERVER['REQUEST_URI'])."view/".$destdir;
	} else {
		echo "error";
	}
//} else {
//	echo "Bitte nur Bilder im Gif bzw. jpg Format hochladen";
//}
?>
