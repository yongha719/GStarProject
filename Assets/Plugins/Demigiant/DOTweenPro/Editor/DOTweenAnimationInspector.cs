// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/03/12 16:03

using System;
using System.Collections.Generic;
using System.IO;
using DG.DemiEditor;
using DG.DOTweenEditor.Core;
using DG.DOTweenEditor.UI;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;
using DOTweenSettings = DG.Tweening.Core.DOTweenSettings;
#if true // UI_MARKER
using UnityEngine.UI;
#endif
#if false // TEXTMESHPRO_MARKER
    using TMPro;
#endif

namespace DG.DOTweenEditor
{
    [CustomEditor(typeof(DOTweenAnimation))]
    public class DOTweenAnimationInspector : ABSAnimationInspector
    {
        enum FadeTargetType
        {
            CanvasGroup,
            Image
        }

        enum ChooseTargetMode
        {
            None,
            BetweenCanvasGroupAndImage
        }

        static readonly Dictionary<DOTweenAnimation.AnimationType, Type[]> _AnimationTypeToComponent = new Dictionary<DOTweenAnimation.AnimationType, Type[]>() {
            { DOTweenAnimation.AnimationType.Move, new[] {
#if true // PHYSICS_MARKER
                typeof(Rigidbody),
#endif
#if true // PHYSICS2D_MARKER
                typeof(Rigidbody2D),
#endif
#if true // UI_MARKER
                typeof(RectTransform),
#endif
                typeof(Transform)
            }},
            { DOTweenAnimation.AnimationType.Rotate, new[] {
#if true // PHYSICS_MARKER
                typeof(Rigidbody),
#endif
#if true // PHYSICS2D_MARKER
                typeof(Rigidbody2D),
#endif
                typeof(Transform)
            }},
            { DOTweenAnimation.AnimationType.LocalMove, new[] { typeof(Transform) } },
            { DOTweenAnimation.AnimationType.LocalRotate, new[] { typeof(Transform) } },
            { DOTweenAnimation.AnimationType.Scale, new[] { typeof(Transform) } },
            { DOTweenAnimation.AnimationType.Color, new[] {
                typeof(Light),
#if true // SPRITE_MARKER
                typeof(SpriteRenderer),
#endif
#if true // UI_MARKER
                typeof(Image), typeof(Text), typeof(RawImage), typeof(Graphic),
#endif
                typeof(Renderer),
            }},
            { DOTweenAnimation.AnimationType.Fade, new[] {
                typeof(Light),
#if true // SPRITE_MARKER
                typeof(SpriteRenderer),
#endif
#if true // UI_MARKER
                typeof(Image), typeof(Text), typeof(CanvasGroup), typeof(RawImage), typeof(Graphic),
#endif
                typeof(Renderer),
            }},
#if true // UI_MARKER
            { DOTweenAnimation.AnimationType.Text, new[] { typeof(Text) } },
#endif
            { DOTweenAnimation.AnimationType.PunchPosition, new[] {
#if true // UI_MARKER
                typeof(RectTransform),
#endif
                typeof(Transform)
            }},
            { DOTweenAnimation.AnimationType.PunchRotation, new[] { typeof(Transform) } },
            { DOTweenAnimation.AnimationType.PunchScale, new[] { typeof(Transform) } },
            { DOTweenAnimation.AnimationType.ShakePosition, new[] {
#if true // UI_MARKER
                typeof(RectTransform),
#endif
                typeof(Transform)
            }},
            { DOTweenAnimation.AnimationType.ShakeRotation, new[] { typeof(Transform) } },
            { DOTweenAnimation.AnimationType.ShakeScale, new[] { typeof(Transform) } },
            { DOTweenAnimation.AnimationType.CameraAspect, new[] { typeof(Camera) } },
            { DOTweenAnimation.AnimationType.CameraBackgroundColor, new[] { typeof(Camera) } },
            { DOTweenAnimation.AnimationType.CameraFieldOfView, new[] { typeof(Camera) } },
            { DOTweenAnimation.AnimationType.CameraOrthoSize, new[] { typeof(Camera) } },
            { DOTweenAnimation.AnimationType.CameraPixelRect, new[] { typeof(Camera) } },
            { DOTweenAnimation.AnimationType.CameraRect, new[] { typeof(Camera) } },
#if true // UI_MARKER
            { DOTweenAnimation.AnimationType.UIWidthHeight, new[] { typeof(RectTransform) } },
#endif
     $��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC$��m��mo�L�D��;�%g�?w��ŷ���ovH0��a�5��*�ؒ��l͛�S�iy�r�O7����%L]��%���hk ����>v1�HB������d\�(eoIx�>3�6BS%���(
��f$�h�����eԎ���H���`ݶf{�Fo�Y���@00uMb�z-��XI$&�gf���7Ӵ�u|'K.�oP
P���F�.��o��9B<~. ����[����<٭�$�����{1�A��.�bKx�L������'�u8n5���e ,]�H����V��Ww�$�C�el��|zys��K�i-�q�ݬbk,wnG��;�� ~�e�r͒���~'1`V⦫�-*[��L�K�'2@����仪��n���2�N� �ƶ�G���i/U��'E�@�`H��;J�������+J�n#���6ڴ�ĹG���N�G�'�Z!�����Wi��NJ�@���A��Z|�[��$q}i�ҷ�QbtTEC= EditorGUILayout.FloatField("Delay", _src.delay);
                if (_src.delay < 0) _src.delay = 0;
                _src.isIndependentUpdate = EditorGUILayout.Toggle("Ignore TimeScale", _src.isIndependentUpdate);
                _src.easeType = EditorGUIUtils.FilteredEasePopup("Ease", _src.easeType);
                if (_src.easeType == Ease.INTERNAL_Custom) {
                    _src.easeCurve = EditorGUILayout.CurveField("   Ease Curve", _src.easeCurve);
                }
                _src.loops = EditorGUILayout.IntField(new GUIContent("Loops", "Set to -1 for infinite loops"), _src.loops);
                if (_src.loops < -1) _src.loops = -1;
                if (_src.loops > 1 || _src.loops == -1)
                    _src.loopType = (LoopType)EditorGUILayout.EnumPopup("   Loop Type", _src.loopType);
                _src.id = EditorGUILayout.TextField("ID", _src.id);

                bool canBeRelative = true;
                // End value and eventual specific options
                switch (_src.animationType) {
                case DOTweenAnimation.AnimationType.Move:
                case DOTweenAnimation.AnimationType.LocalMove:
                    GUIEndValueV3(targetGO, _src.animationType == DOTweenAnimation.AnimationType.Move);
                    _src.optionalBool0 = EditorGUILayout.Toggle("    Snapping", _src.optionalBool0);
                    canBeRelative = !_src.useTargetAsV3;
                    break;
                case DOTweenAnimation.AnimationType.Rotate:
                case DOTweenAnimation.AnimationType.LocalRotate:
                    bool isRigidbody2D = DOTweenModuleUtils.Physics.HasRigidbody2D(_src);
                    if (isRigidbody2D) GUIEndValueFloat();
                    else {
                        GUIEndValueV3(targetGO);
                        _src.optionalRotationMode = (RotateMode)EditorGUILayout.EnumPopup("    Rotation Mode", _src.optionalRotationMode);
                    }
                    break;
                case DOTweenAnimation.AnimationType.Scale:
                    if (_src.optionalBool0) GUIEndValueFloat();
                    else GUIEndValueV3(targetGO);
                    _src.optionalBool0 = EditorGUILayout.Toggle("Uniform Scale", _src.optionalBool0);
                    break;
                case DOTweenAnimation.AnimationType.UIWidthHeight:
                    if (_src.optionalBool0) GUIEndValueFloat();
                    else GUIEndValueV2();
                    _src.optionalBool0 = EditorGUILayout.Toggle("Uniform Scale", _src.optionalBool0);
                    break;
                case DOTweenAnimation.AnimationType.Color:
                    GUIEndValueColor();
                    canBeRelative = false;
                    break;
                case DOTweenAnimation.AnimationType.Fade:
                    GUIEndValueFloat();
                    if (_src.endValueFloat < 0) _src.endValueFloat = 0;
                    if (!_isLightSrc && _src.endValueFloat > 1) _src.endValueFloat = 1;
                    canBeRelative = false;
                    break;
                case DOTweenAnimation.AnimationType.Text:
                    GUIEndValueString();
                    _src.optionalBool0 = EditorGUILayout.Toggle("Rich Text Enabled", _src.optionalBool0);
                    _src.optionalScrambleMode = (ScrambleMode)EditorGUILayout.EnumPopup("Scramble Mode", _src.optionalScrambleMode);
                    _src.optionalString = EditorGUILayout.TextField(new GUIContent("Custom Scramble", "Custom characters to use in case of ScrambleMode.Custom"), _src.optionalString);
                    break;
                case DOTweenAnimation.AnimationType.PunchPosition:
                case DOTweenAnimation.AnimationType.PunchRotation:
                case DOTweenAnimation.AnimationType.PunchScale:
                    GUIEndValueV3(targetGO);
                    canBeRelative = false;
                    _src.optionalInt0 = EditorGUILayout.IntSlider(new GUIContent("    Vibrato", "How much will the punch vibrate"), _src.optionalInt0, 1, 50);
                    _src.optionalFloat0 = EditorGUILayout.Slider(new GUIContent("    Elasticity", "How much the vector will go beyond the starting position when bouncing backwards"), _src.optionalFloat0, 0, 1);
                    if (_src.animationType == DOTweenAnimation.AnimationType.PunchPosition) _src.optionalBool0 = EditorGUILayout.Toggle("    Snapping", _src.optionalBool0);
                    break;
                case DOTweenAnimation.AnimationType.ShakePosition:
                case DOTweenAnimation.AnimationType.ShakeRotation:
                case DOTweenAnimation.AnimationType.ShakeScale:
                    GUIEndValueV3(targetGO);
                    canBeRelative = false;
                    _src.optionalInt0 = EditorGUILayout.IntSlider(new GUIContent("    Vibrato", "How much will the shake vibrate"), _src.optionalInt0, 1, 50);
                    _src.optionalFloat0 = EditorGUILayout.Slider(new GUIContent("    Randomness", "The shake randomness"), _src.optionalFloat0, 0, 90);
                    _src.optionalBool1 = EditorGUILayout.Toggle(new GUIContent("    FadeOut", "If selected the shake will fade out, otherwise it will constantly play with full force"), _src.optionalBool1);
                    if (_src.animationType == DOTweenAnimation.AnimationType.ShakePosition) _src.optionalBool0 = EditorGUILayout.Toggle("    Snapping", _src.optionalBool0);
                    break;
                case DOTweenAnimation.AnimationType.CameraAspect:
                case DOTweenAnimation.AnimationType.CameraFieldOfView:
                case DOTweenAnimation.AnimationType.CameraOrthoSize:
                    GUIEndValueFloat();
                    canBeRelative = false;
                    break;
                case DOTweenAnimation.AnimationType.CameraBackgroundColor:
                    GUIEndValueColor();
                    canBeRelative = false;
                    break;
                case DOTweenAnimation.AnimationType.CameraPixelRect:
                case DOTweenAnimation.AnimationType.CameraRect:
                    GUIEndValueRect();
                    canBeRelative = false;
                    break;
                }

                // Final settings
                if (canBeRelative) _src.isRelative = EditorGUILayout.Toggle("    Relative", _src.isRelative);

                // Events
                AnimationInspectorGUI.AnimationEvents(this, _src);
            }
            EditorGUI.EndDisabledGroup();

            if (GUI.changed) EditorUtility.SetDirty(_src);
        }

        #endregion

        #region Methods

        // Returns TRUE if the Component layout on the src gameObject changed (a Component was added or removed)
        bool ComponentsChanged()
        {
            int prevTotComponentsOnSrc = _totComponentsOnSrc;
            _totComponentsOnSrc = _src.gameObject.GetComponents<Component>().Length;
            return prevTotComponentsOnSrc != _totComponentsOnSrc;
        }

        // Checks if a Component that can be animated with the given animationType is attached to the src
        bool Validate(GameObject targetGO)
        {
            if (_src.animationType == DOTweenAnimation.AnimationType.None) return false;

            Component srcTarget;
            // First check for external plugins
#if false // TK2D_MARKER
            if (_Tk2dAnimationTypeToComponent.ContainsKey(_src.animationType)) {
                foreach (Type t in _Tk2dAnimationTypeToComponent[_src.animationType]) {
                    srcTarget = targetGO.GetComponent(t);
                    if (srcTarget != null) {
                        _src.target = srcTarget;
                        _src.targetType = DOTweenAnimation.TypeToDOTargetType(t);
                        return true;
                    }
                }
            }
#endif
#if false // TEXTMESHPRO_MARKER
            if (_TMPAnimationTypeToComponent.ContainsKey(_src.animationType)) {
                foreach (Type t in _TMPAnimationTypeToComponent[_src.animationType]) {
                    srcTarget = targetGO.GetComponent(t);
                    if (srcTarget != null) {
                        _src.target = srcTarget;
                        _src.targetType = DOTweenAnimation.TypeToDOTargetType(t);
                        return true;
                    }
                }
            }
#endif
            // Then check for regular stuff
            if (_AnimationTypeToComponent.ContainsKey(_src.animationType)) {
                foreach (Type t in _AnimationTypeToComponent[_src.animationType]) {
                    srcTarget = targetGO.GetComponent(t);
                    if (srcTarget != null) {
                        _src.target = srcTarget;
                        _src.targetType = DOTweenAnimation.TypeToDOTargetType(t);
                        return true;
                    }
                }
            }
            return false;
        }

        DOTweenAnimation.AnimationType AnimationToDOTweenAnimationType(string animation)
        {
            if (_datString == null) _datString = Enum.GetNames(typeof(DOTweenAnimation.AnimationType));
            animation = animation.Replace("/", "");
            return (DOTweenAnimation.AnimationType)(Array.IndexOf(_datString, animation));
        }
        int DOTweenAnimationTypeToPopupId(DOTweenAnimation.AnimationType animation)
        {
            return Array.IndexOf(_animationTypeNoSlashes, animation.ToString());
        }

        #endregion

        #region GUI Draw Methods

        void GUIEndValueFloat()
        {
            GUILayout.BeginHorizontal();
            GUIToFromButton();
            _src.endValueFloat = EditorGUILayout.FloatField(_src.endValueFloat);
            GUILayout.EndHorizontal();
        }

        void GUIEndValueColor()
        {
            GUILayout.BeginHorizontal();
            GUIToFromButton();
            _src.endValueColor = EditorGUILayout.ColorField(_src.endValueColor);
            GUILayout.EndHorizontal();
        }

        void GUIEndValueV3(GameObject targetGO, bool optionalTransform = false)
        {
            GUILayout.BeginHorizontal();
            GUIToFromButton();
            if (_src.useTargetAsV3) {
                Transform prevT = _src.endValueTransform;
                _src.endValueTransform = EditorGUILayout.ObjectField(_src.endValueTransform, typeof(Transform), true) as Transform;
                if (_src.endValueTransform != prevT && _src.endValueTransform != null) {
#if true // UI_MARKER
                    // Check that it's a Transform for a Transform or a RectTransform for a RectTransform
                    if (targetGO.GetComponent<RectTransform>() != null) {
                        if (_src.endValueTransform.GetComponent<RectTransform>() == null) {
                            EditorUtility.DisplayDialog("DOTween Pro", "For Unity UI elements, the target must also be a UI element", "Ok");
                            _src.endValueTransform = null;
                        }
                    } else if (_src.endValueTransform.GetComponent<RectTransform>() != null) {
                        EditorUtility.DisplayDialog("DOTween Pro", "You can't use a UI target for a non UI object", "Ok");
                        _src.endValueTransform = null;
                    }
#endif
                }
            } else {
                _src.endValueV3 = EditorGUILayout.Vector3Field("", _src.endValueV3, GUILayout.Height(16));
            }
            if (optionalTransform) {
                if (GUILayout.Button(_src.useTargetAsV3 ? "target" : "value", EditorGUIUtils.sideBtStyle, GUILayout.Width(44))) _src.useTargetAsV3 = !_src.useTargetAsV3;
            }
            GUILayout.EndHorizontal();
#if true // UI_MARKER
            if (_src.useTargetAsV3 && _src.endValueTransform != null && _src.target is RectTransform) {
                EditorGUILayout.HelpBox("NOTE: when using a UI target, the tween will be created during Start instead of Awake", MessageType.Info);
            }
#endif
        }

        void GUIEndValueV2()
        {
            GUILayout.BeginHorizontal();
            GUIToFromButton();
            _src.endValueV2 = EditorGUILayout.Vector2Field("", _src.endValueV2, GUILayout.Height(16));
            GUILayout.EndHorizontal();
        }

        void GUIEndValueString()
        {
            GUILayout.BeginHorizontal();
            GUIToFromButton();
            _src.endValueString = EditorGUILayout.TextArea(_src.endValueString, EditorGUIUtils.wordWrapTextArea);
            GUILayout.EndHorizontal();
        }

        void GUIEndValueRect()
        {
            GUILayout.BeginHorizontal();
            GUIToFromButton();
            _src.endValueRect = EditorGUILayout.RectField(_src.endValueRect);
            GUILayout.EndHorizontal();
        }

        void GUIToFromButton()
        {
            if (GUILayout.Button(_src.isFrom ? "FROM" : "TO", EditorGUIUtils.sideBtStyle, GUILayout.Width(90))) _src.isFrom = !_src.isFrom;
            GUILayout.Space(16);
        }

        #endregion
    }

    // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
    // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
    // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

    [InitializeOnLoad]
    static class Initializer
    {
        static Initializer()
        {
            DOTweenAnimation.OnReset += OnReset;
        }

        static void OnReset(DOTweenAnimation src)
        {
            DOTweenSettings settings = DOTweenUtilityWindow.GetDOTweenSettings();
            if (settings == null) return;

            Undo.RecordObject(src, "DOTweenAnimation");
            src.autoPlay = settings.defaultAutoPlay == AutoPlay.All || settings.defaultAutoPlay == AutoPlay.AutoPlayTweeners;
            src.autoKill = settings.defaultAutoKill;
            EditorUtility.SetDirty(src);
        }
    }
}
