<?php
if(!isset($_FILES['datei']['name'])) {
	for($i=0; $i<=1000; $i++) {
		echo "error<br/>";
	}
	die();
}
$dateityp = GetImageSize($_FILES['datei']['tmp_name']);
//if($dateityp[2] == 3) {
	//if($_FILES['datei']['size'] <  5242880) {
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
		$ssxml->addAttribute('filename', htmlentities($_FILES['datei']['name']));
		$ssxml->addAttribute('ip', $_SERVER['REMOTE_ADDR']);
		$ssxml->addAttribute('timestamp', time());
		$description = $ssxml->addChild('content',$_POST['description'].htmlspecialchars('', ENT_NOQUOTES | ENT_XML1, 'UTF-8'));
		
		move_uploaded_file($_FILES['datei']['tmp_name'], "./".$destdir."/".htmlentities($_FILES['datei']['name']).".big.png");
		
		$img = imagecreatefrompng("./".$destdir."/".htmlentities($_FILES['datei']['name']).".big.png");
		$width = imagesx( $img );
		$height = imagesy( $img );
		if($width > 1024) {
			$new_width = 1024;
			$new_height = floor( $height * ( 1024 / $width ) );
			$tmp_img = imagecreatetruecolor( $new_width, $new_height );
			imagecopyresized( $tmp_img, $img, 0, 0, 0, 0, $new_width, $new_height, $width, $height );
			imagepng( $tmp_img, "./".$destdir."/".htmlentities($_FILES['datei']['name']).".small.png" );
		} else {
			copy("./".$destdir."/".htmlentities($_FILES['datei']['name']).".big.png", "./".$destdir."/".htmlentities($_FILES['datei']['name']).".small.png");
		}
		$new_width = 260;
		$new_height = floor( $height * ( 260 / $width ) );
  
		// create a new temporary image
		$tmp_img = imagecreatetruecolor( $new_width, $new_height );
  
		// copy and resize old image into new image 
		imagecopyresized( $tmp_img, $img, 0, 0, 0, 0, $new_width, $new_height, $width, $height );
  
		// save thumbnail into a file
		imagepng( $tmp_img, "./".$destdir."/".htmlentities($_FILES['datei']['name']).".thumb.png" );

		$rgb = colorPalette("./".$destdir."/".htmlentities($_FILES['datei']['name']).".thumb.png", 1, 4);
		
		$col = imageColor::averageImage($tmp_img);
		
		$compl = comp($col['red'], $col['green'], $col['blue']);
		
		$ssxml->addAttribute('comphexcolor', RGBToHex($col['red'],$col['green'],$col['blue']));
		
		$ssxml->addAttribute('hexcolor', RGBToHex($col['red'],$col['green'],$col['blue']));
		
		$ssxml->asXML("./".$destdir."/screenshot.xml");
		
		echo "http://".$_SERVER['SERVER_NAME'].dirname($_SERVER['REQUEST_URI'])."view/".$destdir;
	//} else {
	//	echo "error";
	//}
//} else {
//	echo "Bitte nur Bilder im Gif bzw. jpg Format hochladen";
//}
function comp($r, $g, $b, $shift=192) { 
	$rgb['red'] = ((hexdec($r)+$shift)%256);
	$rgb['green'] = ((hexdec($g)+$shift)%256);
	$rgb['blue'] = ((hexdec($b)+$shift)%256);
	return $rgb;
}
function RGBToHex($r, $g, $b) {
		//String padding bug found and the solution put forth by Pete Williams (http://snipplr.com/users/PeteW)
		$hex = "#";
		$hex.= str_pad(dechex($r), 2, "0", STR_PAD_LEFT);
		$hex.= str_pad(dechex($g), 2, "0", STR_PAD_LEFT);
		$hex.= str_pad(dechex($b), 2, "0", STR_PAD_LEFT);
 
		return $hex;
}
function colorPalette($imageFile, $numColors, $granularity = 5) 
{ 
   $granularity = max(1, abs((int)$granularity)); 
   $colors = array(); 
   $size = @getimagesize($imageFile); 
   if($size === false) 
   { 
      user_error("Unable to get image size data"); 
      return false; 
   } 
   $img = @imagecreatefrompng($imageFile);
   // Andres mentioned in the comments the above line only loads jpegs, 
   // and suggests that to load any file type you can use this:
   // $img = @imagecreatefromstring(file_get_contents($imageFile)); 

   if(!$img) 
   { 
      user_error("Unable to open image file"); 
      return false; 
   } 
   for($x = 0; $x < $size[0]; $x += $granularity) 
   { 
      for($y = 0; $y < $size[1]; $y += $granularity) 
      { 
         $thisColor = imagecolorat($img, $x, $y); 
         $rgb = imagecolorsforindex($img, $thisColor); 
         $red = round(round(($rgb['red'] / 0x33)) * 0x33); 
         $green = round(round(($rgb['green'] / 0x33)) * 0x33); 
         $blue = round(round(($rgb['blue'] / 0x33)) * 0x33); 
         $thisRGB = sprintf('%02X%02X%02X', $red, $green, $blue); 
         if(array_key_exists($thisRGB, $colors)) 
         { 
            $colors[$thisRGB]++; 
         } 
         else 
         { 
            $colors[$thisRGB] = 1; 
         } 
      } 
   } 
   arsort($colors); 
   return array_slice(array_keys($colors), 0, $numColors); 
} 
class imageColor
{
	function scanLine($image, $height, $width, $axis, $line)
	{
		$i = 0;
		
		if("x" == $axis){
			$limit = $width;
			$y = $line;
			$x =& $i;
			
			if(-1 == $line){
				$y = 0;
				$y2 = $width -1;
				$x2 =& $i;	
			}
		} else {
			$limit = $height;
			$x = $line;
			$y =& $i;
			
			if(-1 == $line){
				$x = 0;
				$x2 = $width -1;
				$y2 =& $i;	
			}
		}
		
		$colors = array();
		
		if(-1 == $line){
			for($i = 0; $i < $limit; $i++){
				self::addPixel($colors, $image, $x, $y);
				self::addPixel($colors, $image, $x2, $y2);
			}
		} else {
			for($i = 0; $i < $limit; $i++){
				self::addPixel($colors, $image, $x, $y);
			}
		}
		
		return $colors;
	}
	
	function addPixel(&$colors, $image, $x, $y)
	{
		$rgb = imagecolorat($image, $x, $y);
		$color = imagecolorsforindex($image, $rgb);
		$colors['red'][] = $color['red'];
		$colors['green'][] = $color['green'];
		$colors['blue'][] = $color['blue'];
	}
	
	function totalColors($color, $colors)
	{
		$color['red'] += array_sum($colors['red']);
		$color['green'] += array_sum($colors['green']);
		$color['blue'] += array_sum($colors['blue']);
 
		return $colors;
	}
	
	function averageTotal($color, $count)
	{
		$color['red'] = intval($color['red']/$count);
		$color['green'] = intval($color['green']/$count);
		$color['blue'] = intval($color['blue']/$count);
		
		return $color;
	}
	
	function averageResize($image)
	{
		$width = imagesx($image);
		$height = imagesy($image);
		
		$pixel = imagecreatetruecolor(1, 1);
		imagecopyresampled($pixel, $image, 0, 0, 0, 0, 1, 1, $width, $height);
		$rgb = imagecolorat($pixel, 0, 0);
		$color = imagecolorsforindex($pixel, $rgb);
		
		return $color;
	}
	
	function averageBorder($image)
	{
		$width = imagesx($image);
		$height = imagesy($image);
		
		$colors = self::scanLine($image, $height, $width, 'x', -1);
		self::totalColors(&$color, $colors);
		
		$colors = self::scanLine($image, $height, $width, 'y', -1);
		self::totalColors(&$color, $colors);
 
		$borderSize = ($height=$width)*2;
		self::averageTotal(&$color, $borderSize);
		
		return $color;
	}
	
	function averageImage($image)
	{
		$width = imagesx($image);
		$height = imagesy($image);
		
		$colors = array();
		
		for($line = 0; $line < $height; $line++){
			$colors = self::scanLine($image, $height, $width, 'x', $line);
			self::totalColors(&$color, $colors);
		}
		
		$count = $width*$height;
		self::averageTotal(&$color, $count);
		
		return $color;
	}
}
?>
