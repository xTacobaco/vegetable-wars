<?php
  /* [REQUEST CODES]
   * 0: wrong creds!
   * 1: ok/granted!
   * 2: input error!
   * 3: user exists!
   */
  extract($_POST);
  if (isset($seq) && $seq == 'QWERTY') {
    if (isset($status)) {
      $db = new PDO('mysql:host=localhost;dbname=vw', 'root', '');
      if (isset($user) && isset($pass)) {
        if(strlen($user) < 4 || strlen($user) > 16) die('2');
        if(strlen($pass) < 4 || strlen($pass) > 16) die('2');
        $pass = hash('sha512', $pass . $user);

        switch ($status) {
          case 'log':
            $query = $db->prepare('SELECT * FROM users WHERE username = ? AND password = ?');
            $query->execute([$user, $pass]);
            die($query->rowCount() ? '1' : '0');
            break;
          case 'reg':
            $query = $db->prepare('SELECT * FROM users WHERE username = ?');
            $query->execute([$user]);
            if($query->rowCount()) die('3');

            $query = $db->prepare('INSERT INTO users (username, password) VALUES (?, ?)');
            $query->execute([$user, $pass]);
            die('1');
            break;
        }
      } else {
        die('2');
      }
    } else {
      die('2');
    }
  } else {
    die('2');
  }
 ?>
