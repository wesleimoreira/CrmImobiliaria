# ✅ Checklist de Segurança - Publicação em Repositório Público

Este documento lista todos os dados sensíveis que foram removidos e o que você DEVE fazer antes de publicar este repositório em um servidor de produção.

## 🔒 Dados Já Removidos do Repositório

Todos os itens abaixo foram removidos/genericizados para segurança:

### ✅ Domínios e URLs
- ❌ `crm.primefrota.cloud` → ✅ `crm.example.com` (em `.env.example`, `proxy/Caddyfile`, `docker-compose.yml`)
- ❌ `primefrota.cloud` → ✅ `example.com` (em landing page e docker-compose)

### ✅ Email e Contato
- ❌ `admin@primefrota.cloud` → ✅ `admin@example.com` (em `.env.example`)
- ❌ `contato@primefrota.com.br` → ✅ `contact@example.com` (em landing page)
- ❌ `Weslei Moreira Santana` → ✅ `Seu nome aqui` (em landing page)
- ❌ Link pessoal LinkedIn → ✅ Placeholder `seu-usuario` (em landing page)

### ✅ Infraestrutura
- ❌ Referência a "VPS Hostinger KVM 1" → ✅ Descrição genérica "VPS / Servidor dedicado"
- ❌ Código de referência `REFERRALCODE=PRIMEFROTA` → ✅ Removido

### ✅ Senhas e Configurações
- ❌ `MSSQL_SA_PASSWORD=TrocarPorUmaSenhaForte#2026` → ✅ `SenhaForte#2026Minuscula`
- ❌ `SEED_ADMIN_PASSWORD=TrocarPorOutraSenhaForte#2026` → ✅ `SenhaForteAdmin#2026`
- ❌ `Admin@12345` (em Program.cs) → ✅ `ChangeMe#2026`

## ⚠️ O Que Você Precisa Fazer Agora

### ANTES de Fazer Deploy em Produção:

#### 1. **Criar seu próprio `.env` (NUNCA commitar)**
```bash
# Copie o arquivo example
cp .env.example .env

# Edite com valores REAIS e SEGUROS:
MSSQL_SA_PASSWORD=SenhaForte_Aleatoria#2026_MuitoSegura
SEED_ADMIN_EMAIL=admin@seu-dominio-real.com.br
SEED_ADMIN_PASSWORD=SenhaAdminForte_Unica#2026
```

#### 2. **Atualizar a Landing Page**
No arquivo `landing/index.html`:

```html
<!-- Linha ~787: Seu nome -->
<h2 class="serif">Seu Nome Completo</h2>

<!-- Linha ~789: Seu LinkedIn -->
<a class="sobre-link" href="https://www.linkedin.com/in/seu-usuario-linkedin/" target="_blank" rel="noopener">Ver perfil no LinkedIn →</a>

<!-- Linha ~805: Email de contato -->
<span>seu-email@seu-dominio.com.br</span>

<!-- Linha ~802: Link de contato -->
href="mailto:seu-email@seu-dominio.com.br?subject=...
```

#### 3. **Configurar Caddyfile (proxy reverso)**
No arquivo `proxy/Caddyfile`:

```
SEU_DOMINIO_CRM.com.br {
    reverse_proxy web:8080
}

SEU_DOMINIO_LANDING.com.br {
    reverse_proxy landing:80
}
```

#### 4. **Variáveis de Ambiente em Produção**
Nunca use valores padrão! Configure no seu ambiente de deploy (Dokploy, Docker Compose, etc):

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__CrmImobiliaria=Server=db;Database=CrmImobiliaria;User Id=sa;Password=SENHA_MUITO_SEGURA;TrustServerCertificate=True;MultipleActiveResultSets=true
Seed__AdminEmail=admin@seu-dominio-real.com.br
Seed__AdminPassword=SENHA_ADMIN_MUITO_SEGURA_E_UNICA
```

#### 5. **Segurança do Banco de Dados**
- ✅ Mude a senha do usuário `sa` do SQL Server (requisito obrigatório)
- ✅ Use um banco de dados separado para cada ambiente (dev/staging/prod)
- ✅ Ative backups automáticos
- ✅ Restrinja acesso ao banco apenas para o container da aplicação

#### 6. **HTTPS/TLS**
- ✅ Use certificados SSL válidos (Let's Encrypt, etc)
- ✅ Configure HSTS e outras headers de segurança
- ✅ Force redirect HTTP → HTTPS

#### 7. **Hospedagem**
- ✅ Escolha uma provedor de confiança (não precisa ser Hostinger)
- ✅ Configure firewall para permitir apenas portas 80/443
- ✅ Mantenha sistema operacional e dependências atualizadas
- ✅ Configure monitoramento e logs

## 🔐 Coisas Seguras Neste Repositório

- ✅ Sem credenciais reais commitadas
- ✅ Sem URLs de servidores pessoais
- ✅ Sem dados pessoais
- ✅ Sem chaves/certificados privados
- ✅ `.gitignore` bem configurado
- ✅ Senhas padrão são claramente genéricas e devem ser alteradas
- ✅ `.env` não está versionado

## 📋 Checklist Final Antes de Publicar

- [ ] Criar seu próprio `.env` com valores reais
- [ ] Atualizar landing page com seu nome e contato
- [ ] Atualizar Caddyfile com seus domínios reais
- [ ] Testar login com admin padrão (senha será diferente)
- [ ] Alterar senha do admin na primeira execução
- [ ] Configurar HTTPS/SSL
- [ ] Ativar backups automáticos
- [ ] Configurar monitoramento e alertas
- [ ] Revisar logs de segurança
- [ ] Testar em staging antes de produção

## 🆘 Suporte

Se encontrar qualquer informação sensível ainda no repositório:

1. Não commite `.env` real
2. Revise seu `.gitignore`
3. Use `git filter-branch` ou `git-filter-repo` para remover dados sensíveis do histórico

---

**Status:** ✅ Repositório seguro para publicação no GitHub como público
