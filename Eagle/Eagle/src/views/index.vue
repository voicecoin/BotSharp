<style scoped lang="less">
    .index{
        width: 100%;
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        text-align: center;
        h1{
            height: 200px;
            img{
                height: 100%;
            }
        }
        h2{
            color: #666;
            margin-bottom: 200px;
            p{
                margin: 0 0 50px;
            }
        }
        .ivu-row-flex{
            height: 100%;
        }
    }
</style>
<template>
    <div class="index">
        <Row type="flex" justify="center" align="middle">
            <Col span="24">
                <h1>
                    <img src="../images/logo.png">
                </h1>
					<br />
                <h2>
                    <p>YAYABOT 管理平台</p>
					
					<Form ref="formInline" :model="formInline" :rules="ruleInline" inline>
						<Form-item prop="username">
							<Input type="text" v-model="formInline.username" placeholder="邮箱">
								<Icon type="ios-person-outline" slot="prepend"></Icon>
							</Input>
						</Form-item>
						<Form-item prop="password">
							<Input type="password" v-model="formInline.password" placeholder="密码">
								<Icon type="ios-locked-outline" slot="prepend"></Icon>
							</Input>
						</Form-item>
						<Form-item>
							<Button type="primary" @click="handleSubmit('formInline')">登录</Button>
						</Form-item>
					</Form>
                </h2>
				
				<span>© 2012-2017 YayaBot — 深圳爱用科技有限公司</span>
            </Col>

			
        </Row>
    </div>
</template>
<script>
	import {HTTP} from '../libs/http-common';
	
    export default {
		data () {
            return {
                formInline: {
                    username: 'info@yaya.ai',
                    password: 'Yayabot123'
                },
                ruleInline: {
                    username: [
                        { required: true, message: '请填写用户名', trigger: 'blur' }
                    ],
                    password: [
                        { required: true, message: '请填写密码', trigger: 'blur' },
                        { type: 'string', min: 6, message: '密码长度不能小于6位', trigger: 'blur' }
                    ]
                }
            }
        },
        methods: {
			handleSubmit(name) {
                this.$refs[name].validate((valid) => {
                    if (valid) {
                        this.getToken();
                    } else {
                        this.$Message.error('表单验证失败!');
                    }
                })
            },
            getToken() {
				var qs = require('qs');
				
                HTTP.post(`/token`, qs.stringify(this.formInline), { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
					.then(response => {
						localStorage.setItem('token', response.data.token);
						window.location.href = '/dashboard';
					})
					.catch(e => {
					  this.$Message.error('用户身份验证失败!');
					})
            }
        }
    }
</script>
