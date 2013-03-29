<?php
$dateityp = GetImageSize($_FILES['datei']['tmp_name']);
//if($dateityp[2] == 3) {
	if($_FILES['datei']['size'] <  1572864) {
		move_uploaded_file($_FILES['datei']['tmp_name'], "upload/".$_FILES['datei']['name']);
		echo "upload/".$_FILES['datei']['name'];
	} else {
		echo "Das Bild darf nicht größer als 1,5 MB sein";
	}
//} else {
//	echo "Bitte nur Bilder im Gif bzw. jpg Format hochladen";
//}
?>
