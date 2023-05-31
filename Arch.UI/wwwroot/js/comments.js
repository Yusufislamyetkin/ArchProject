$(document).ready(function () {
    // Yorum formunun submit olayını dinliyoruz
    $('#comment-submit').click(function (event) {
        event.preventDefault(); // Formun otomatik olarak gönderilmesini engelliyoruz

        // Formdan gerekli bilgileri alıyoruz
        var comment = $('#comment-input').val();

        // Yorum ekleme isteğini gönderiyoruz
        $.ajax({
            url: '/Blog/EkleYorum',
            type: 'POST',
            data: { comment: comment },
            success: function (response) {
                // Başarılı bir yanıt alındığında, yeni yorumu sayfaya ekliyoruz
                var newComment = '<div class="d-flex flex-row p-3">' +
                    '<img src="https://i.imgur.com/zQZSWrt.jpg" width="40" height="40" class="rounded-circle mr-3">' +
                    '<div class="w-100">' +
                    '<div class="d-flex justify-content-between align-items-center">' +
                    '<div class="d-flex flex-row align-items-center">' +
                    '<span class="mr-2">Your Name</span>' +
                    '<small class="c-badge">New Comment</small>' +
                    '</div>' +
                    '<small>Just now</small>' +
                    '</div>' +
                    '<p class="text-justify comment-text mb-0">' + response.Content + '</p>' +
                    '</div>' +
                    '</div>';

                $('#comments-container').append(newComment); // Yeni yorumu ekliyoruz
                $('#comment-input').val(''); // Yorum girişini temizliyoruz
            },
            error: function () {
                alert('Yorum eklenirken bir hata oluştu.'); // Hata durumunda bir uyarı gösteriyoruz
            }
        });
    });
});